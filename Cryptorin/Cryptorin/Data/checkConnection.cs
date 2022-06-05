using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Cryptorin.Data
{
    /// <summary>
    /// class for checking the connection with the server
    /// </summary>
    public class CheckConnection
    {
        /// <summary>
        /// Check connection with server
        /// </summary>
        /// <param name="strServer">Server address</param>
        /// <returns></returns>
        public bool ConnectionAvailable(string strServer)
        {
            try
            {
                HttpWebRequest reqFP = (HttpWebRequest)HttpWebRequest.Create(strServer);

                HttpWebResponse rspFP = (HttpWebResponse)reqFP.GetResponse();
                if (HttpStatusCode.OK == rspFP.StatusCode)
                {
                    // HTTP = 200 - there is internet
                    rspFP.Close();
                    return true;
                }
                else
                {
                    // there is no internet or the server is unavailable
                    rspFP.Close();
                    return false;
                }
            }
            catch (Exception)
            {
                // there is no internet
                return false;
            }
        }
    }
}
