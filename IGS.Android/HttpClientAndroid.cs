using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IGS.Droid;
using IGS.Services;
using Javax.Net.Ssl;
using Xamarin.Android.Net;

[assembly: Xamarin.Forms.Dependency(typeof(HttpClientAndroid))]
namespace IGS.Droid
{
    public class HttpClientAndroid : IHttpClientHandler
    {
        public HttpClient GetHttpClient()
        {
            var client = new HttpClient(new IgnoreSSLClientHandler());
            return client;
        }

        internal class IgnoreSSLClientHandler : AndroidClientHandler
        {
            protected override SSLSocketFactory ConfigureCustomSSLSocketFactory(HttpsURLConnection connection)
            {
                return SSLCertificateSocketFactory.GetInsecure(1000, null);
            }
            protected override IHostnameVerifier GetSSLHostnameVerifier(HttpsURLConnection connection)
            {
                return new IgnoreSSLHostnameVerifier();
            }
        }
        internal class IgnoreSSLHostnameVerifier : Java.Lang.Object, IHostnameVerifier
        {
            public bool Verify(string hostname, ISSLSession session)
            {
                return true;
            }
        }
    }
}