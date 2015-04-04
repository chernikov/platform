using platformAthletic.Global.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace platformAthletic.Tools
{
    public class AuthorizeProcessor
    {
        public static string GetRequest(AuthorizeSettings authorizeSetting, string cardNumber, string expDate, 
            string cvc, double amount, string description, string firstName,
            string lastName, string address, string state, string zipCode)
        {
            // By default, this sample code is designed to post to our test server for
            // developer accounts: https://test.authorize.net/gateway/transact.dll
            // for real accounts (even in test mode), please make sure that you are
            // posting to: https://secure.authorize.net/gateway/transact.dll
            

            Dictionary<string, string> post_values = new Dictionary<string, string>();
            //the API Login ID and Transaction Key must be replaced with valid values
            post_values.Add("x_login", authorizeSetting.LoginID);
            post_values.Add("x_tran_key", authorizeSetting.TransactionKey);
            post_values.Add("x_delim_data", "TRUE");
            post_values.Add("x_delim_char", "|");
            post_values.Add("x_relay_response", "FALSE");

            post_values.Add("x_type", "AUTH_CAPTURE");
            post_values.Add("x_method", "CC");
            post_values.Add("x_card_num", cardNumber);
            post_values.Add("x_exp_date", expDate);
            post_values.Add("x_card_code", cvc);

            post_values.Add("x_amount", amount.ToString("0.00"));
            post_values.Add("x_description", description);

            post_values.Add("x_first_name", firstName);
            post_values.Add("x_last_name", lastName);
            post_values.Add("x_address", address);
            post_values.Add("x_state", state);
            post_values.Add("x_zip",zipCode);

            // Additional fields can be added here as outlined in the AIM integration
            // guide at: http://developer.authorize.net

            // This section takes the input fields and converts them to the proper format
            // for an http post.  For example: "x_login=username&x_tran_key=a1B2c3D4"
            String post_string = "";

            foreach (KeyValuePair<string, string> post_value in post_values)
            {
                post_string += post_value.Key + "=" + HttpUtility.UrlEncode(post_value.Value) + "&";
            }
            post_string = post_string.TrimEnd('&');

            // The following section provides an example of how to add line item details to
            // the post string.  Because line items may consist of multiple values with the
            // same key/name, they cannot be simply added into the above array.
            //
            // This section is commented out by default.
            /*
            string[] line_items = {
                "item1<|>golf balls<|><|>2<|>18.95<|>Y",
                "item2<|>golf bag<|>Wilson golf carry bag, red<|>1<|>39.99<|>Y",
                "item3<|>book<|>Golf for Dummies<|>1<|>21.99<|>Y"};
	
            foreach( string value in line_items )
            {
                post_string += "&x_line_item=" + HttpUtility.UrlEncode(value);
            }
            */

            // create an HttpWebRequest object to communicate with Authorize.net
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(authorizeSetting.Url);
            objRequest.Method = "POST";
            objRequest.ContentLength = post_string.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";

            // post data is sent as a stream
            StreamWriter myWriter = null;
            myWriter = new StreamWriter(objRequest.GetRequestStream());
            myWriter.Write(post_string);
            myWriter.Close();

            // returned values are returned as a stream, then read into a string
            String post_response;
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader responseStream = new StreamReader(objResponse.GetResponseStream()))
            {
                post_response = responseStream.ReadToEnd();
                responseStream.Close();
            }

            return post_response;
        }

    }
}