using System;

namespace PayPalHandler
{
	public class PaypalConfiguration
	{		
		Ini m_objINI = null;
		//string m_strINIFile = "PorticoPaypalHandler.ini";
		string m_strSectionPaypalInfo = "PaypalInfo";
		string m_strEntryPaypalUrl = "PaypalUrl";
		string m_strEntryEmailAccount = "EmailAccount";
		string m_strEntryLogoUrl = "LogoUrl";
		string m_strEntrySuccessUrl = "SuccessUrl";
		string m_strEntryCancelUrl = "CancelUrl";
		string m_strEntryNotifyUrl = "NotifyUrl";

		/* Paypal - Business Acoount */
		private string m_strAccountEmail = "PayPalEmailAccount@YourCompany.com";

		/* Using Sandbox Paypal for testing only */
		private string m_strPayPalUrl = "https://www.sandbox.paypal.com/us/cgi-bin/webscr?";		
		/* Using Live Paypal for the real transactions */
		//private string PayPalUrl = "https://www.paypal.com/cgi-bin/webscr?";
		private string m_strLogoUrl = String.Empty;
		private string m_strSuccessUrl = String.Empty;
		private string m_strCancelUrl = String.Empty;
		/* Activate IPN option on the Paypal payment gateway */
		private string m_strNotifyUrl = String.Empty;

		// PaypalUrl	
		public string PaypalUrl
		{
			get 
			{ 
				return m_strPayPalUrl; 
			}
			set 
			{ 
				m_strPayPalUrl = value; 
			}
		}
		
		// AccountEmail
		public string AccountEmail
		{
			get 
			{ 
				return m_strAccountEmail; 
			}
			set 
			{ 
				m_strAccountEmail = value; 
			}
		}

		// LogoUrl
		public string LogoUrl
		{
			get 
			{ 
				return m_strLogoUrl; 
			}
			set 
			{ 
				m_strLogoUrl = value; 
			}
		}

		// SuccessUrl
		public string SuccessUrl
		{
			get 
			{ 
				return m_strSuccessUrl; 
			}
			set 
			{ 
				m_strSuccessUrl = value; 
			}
		}

		// CancelUrl
		public string CancelUrl
		{
			get 
			{ 
				return m_strCancelUrl; 
			}
			set 
			{ 
				m_strCancelUrl = value; 
			}
		}

		// NotifyUrl
		public string NotifyUrl
		{
			get 
			{ 
				return m_strNotifyUrl; 
			}
			set 
			{ 
				m_strNotifyUrl = value; 
			}
		}


		/* Constructor */
		public PaypalConfiguration(string p_strConfigFile)
		{	
			try
			{		
				m_objINI = new Ini(p_strConfigFile);
			}
			catch(Exception ex) 
			{
				throw new Exception("The INI file is not found" , ex);
			}
			
			m_strAccountEmail = String.Empty;;
			m_strPayPalUrl = String.Empty;;		
			m_strLogoUrl = String.Empty;
			m_strSuccessUrl = String.Empty;
			m_strCancelUrl = String.Empty;
			m_strNotifyUrl = String.Empty;
		}

		/* Read the INI file */
		public void readConfigFile()
		{
			PaypalUrl = (string)m_objINI.GetValue(m_strSectionPaypalInfo, m_strEntryPaypalUrl);
			AccountEmail = (string)m_objINI.GetValue(m_strSectionPaypalInfo, m_strEntryEmailAccount);
			LogoUrl = (string)m_objINI.GetValue(m_strSectionPaypalInfo, m_strEntryLogoUrl);
			SuccessUrl = (string)m_objINI.GetValue(m_strSectionPaypalInfo, m_strEntrySuccessUrl);
			CancelUrl = (string)m_objINI.GetValue(m_strSectionPaypalInfo, m_strEntryCancelUrl);
			NotifyUrl = (string)m_objINI.GetValue(m_strSectionPaypalInfo, m_strEntryNotifyUrl);
		}

		/* Write the INI file */
		public void writeConfigFile()
		{
			m_objINI.SetValue(m_strSectionPaypalInfo, m_strEntryPaypalUrl, PaypalUrl);
			m_objINI.SetValue(m_strSectionPaypalInfo, m_strEntryEmailAccount, AccountEmail);
			m_objINI.SetValue(m_strSectionPaypalInfo, m_strEntryLogoUrl, LogoUrl);
			m_objINI.SetValue(m_strSectionPaypalInfo, m_strEntrySuccessUrl, SuccessUrl);
			m_objINI.SetValue(m_strSectionPaypalInfo, m_strEntryCancelUrl, CancelUrl);
			m_objINI.SetValue(m_strSectionPaypalInfo, m_strEntryNotifyUrl, NotifyUrl);			
		}

	}
}
