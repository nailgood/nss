using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Hammock;
using Hammock.Authentication.OAuth;

namespace Twitter
{
	/// <summary>
	/// This class is used for pushing data to the Twitter API (such as updating statuses)
	/// </summary>
	public class TwitterPush
	{
		#region Properties

		/// <summary>
		/// Gets the OAuth credentials created when the class is initialised
		/// </summary>
		public OAuthCredentials Credentials { get; private set; }

		#endregion Properties

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="TwitterPush"/> class for writing to the Twitter API
		/// </summary>
		/// <param name="data">An instance of a loaded IOAuthData object</param>
		public TwitterPush(OAuthData data)
		{
			if (data == null)
				throw new ArgumentNullException("data", "The IOAuthData was null - you need to pass a loaded instance");

			System.Net.ServicePointManager.Expect100Continue = false;
			// See http://blogs.msdn.com/b/shitals/archive/2008/12/27/9254245.aspx

			OAuthCredentials credentials = new OAuthCredentials()
			{
				Type = OAuthType.AccessToken,
				SignatureMethod = OAuthSignatureMethod.HmacSha1,
				ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
				ConsumerKey = data.ConsumerKey,
				ConsumerSecret = data.ConsumerSecret,
				Token = data.Token,
				TokenSecret = data.TokenSecret,
				Version = TwitterConstants.Version
			};

			this.Credentials = credentials;
		}

		#endregion Constructor

		/// <summary>
		/// Checks whether the current credentials are valid and authorised for the consuming app
		/// </summary>
		/// <remarks>
		/// See http://dev.twitter.com/doc/get/account/verify_credentials
		/// </remarks>
		/// <returns>True if credentials are OK or False if not</returns>
		public bool CredentialsAreValid()
		{
			bool valid = false;

			try
			{
				RestClient client = new RestClient
				{
					Authority = TwitterConstants.Authority,
					VersionPath = TwitterConstants.Version
				};

				RestRequest request = new RestRequest
				{
					Credentials = this.Credentials,
					Path = "account/verify_credentials.xml"
				};

				var response = client.Request(request);

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					valid = true;
				}
			}
			catch (ArgumentException ex)
			{
				// Can happen if no credentials ever supplied
				return false;
			}

			return valid;
		}

		/// <summary>
		/// Updates the Twitter status for the authenticated user 
		/// </summary>
		/// <param name="message">The message to be posted to the status (if more than 140 characters will be truncated)</param>
		/// <param name="encode">if set to <c>true</c> then HTML Encode the message (normally you would do this unless your message is already encoded)</param>
		/// <returns>The response stream returned via the API</returns>
		/// <remarks>
		/// See http://dev.twitter.com/doc/post/statuses/update
		/// </remarks>
		public TwitterResponse UpdateStatus(string message, bool encode)
		{
			RestClient client = new RestClient
			{
				Authority = TwitterConstants.Authority,
				VersionPath = TwitterConstants.Version
			};

			if (encode)
				message = HttpUtility.HtmlEncode(message);

			client.AddField("status", message);

			RestRequest request = new RestRequest
			{
				Credentials = this.Credentials,
				Path = "statuses/update.xml"
			};

			return new TwitterResponse(client.Request(request));
		}


		/// <summary>
		/// Updates the Twitter status for the authenticated user
		/// </summary>
		/// <param name="message">The message to be posted to the status (must be less than 140 characters)</param>
		/// <returns>The response stream returned via the API</returns>
		/// <remarks>
		/// See http://dev.twitter.com/doc/post/statuses/update
		/// </remarks>
		public TwitterResponse UpdateStatus(string message)
		{
			return UpdateStatus(message, true);
		}


	}
}
