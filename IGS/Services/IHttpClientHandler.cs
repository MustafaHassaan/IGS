using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace IGS.Services
{
    public interface IHttpClientHandler
    {
        HttpClient GetHttpClient();
    }
}
