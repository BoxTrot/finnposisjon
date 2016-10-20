﻿using System.IO;
using System.Net;
using System.Text;
using System.Web.Helpers;
using Kartverket.FinnPosisjon.Models;

namespace Kartverket.FinnPosisjon.Services
{
    public class CoordinateTransformer
    {
        public static Coordinates Transform(Coordinates coordinates, int coordinateSystemSosiCode,
            int resultCoordinateSystemSosiCode)
        {
            const string parameterizedWebServiceUrl =
                "http://www.norgeskart.no/ws/trans.py?ost={0}&nord={1}&sosiKoordSys={2}&resSosiKoordSys={3}";

            var callReadyUrl = string.Format(parameterizedWebServiceUrl, coordinates.X, coordinates.Y,
                coordinateSystemSosiCode, resultCoordinateSystemSosiCode);

            var json = GetJsonWebServiceResponse(callReadyUrl);

            var transformationResponse = Json.Decode<TransformationResponse>(json);

            return transformationResponse.ErrKode == 0
                ? new Coordinates
                {
                    X = transformationResponse.Ost,
                    Y = transformationResponse.Nord
                }
                : null;
        }

        private static string GetJsonWebServiceResponse(string url)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);

            try
            {
                var response = request.GetResponse();

                using (var responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                var errorResponse = ex.Response;

                using (var responseStream = errorResponse.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    var errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }
    }

    public class TransformationResponse
    {
        public int ErrKode { get; set; }
        public int SosiKoordSys { get; set; }
        public double Ost { get; set; }
        public double Hoyde { get; set; }
        public double Nord { get; set; }
        public string ErrTekst { get; set; }
    }
}
