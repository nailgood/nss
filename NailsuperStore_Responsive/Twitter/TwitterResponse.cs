using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Hammock;

namespace Twitter
{
	/// <summary>
	/// Abstracts the response returned from a Twitter API request via Hammock into more usable properties
	/// </summary>
	/// <remarks>
	/// You could probably use this as a base class for all responses
	/// </remarks>
	public class TwitterResponse
	{
		private XDocument _xmlDoc = null;

		/// <summary>
		/// Gets the REST response stream from the Twitter API
		/// </summary>
		public RestResponse Response { get; private set; }

		/// <summary>
		/// Gets a value indicating whether the server returned a HTTP Status of 200 OK
		/// </summary>
		public bool IsOk 
		{
			get
			{
				if (this.Response == null)
					return false;

				return Response.StatusCode == HttpStatusCode.OK;
			}
		}

		/// <summary>
		/// Gets an XDocument XML representation of the response content
		/// </summary>
		public XDocument XmlDoc
		{
			get
			{
				if (this.Response == null)
					return null;

				if (_xmlDoc == null && !String.IsNullOrEmpty(this.Response.Content))
				{
					_xmlDoc = XDocument.Parse(this.Response.Content);
				}

				return _xmlDoc;
			}
		}

		/// <summary>
		/// Gets the error message returned by API when the status is not OK
		/// </summary>
		public string ErrorMessage
		{
			get
			{
				if (!this.IsOk)
				{
					if (this.Response == null)
						return "The response stream was null";

					return this.XmlDoc.Descendants().Where(x => x.Name == "error").Select(x => x.Value).SingleOrDefault();
				}
				else
				{
					return String.Empty;
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UpdateResponse"/> class with a REST response
		/// </summary>
		/// <param name="response">The REST response from the Twitter Update Status</param>
		public TwitterResponse(RestResponse response)
		{
			this.Response = response;
		}

	}
}
