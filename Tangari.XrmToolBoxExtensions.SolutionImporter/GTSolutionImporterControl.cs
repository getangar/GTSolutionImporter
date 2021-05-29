using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using System.IO;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Pfe.Xrm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Xml;

namespace Tangari.XrmToolBoxExtensions.SolutionImporter
{
    public partial class GTSolutionImporterControlPlugin : PluginControlBase, IHelpPlugin, IPayPalPlugin, IGitHubPlugin
    {
        #region Private parameters

        private Boolean isSolutionSelected = false;
        private String solutionLocation = String.Empty;

        #endregion

        #region Implementation of the IHelpPlugin interface

        public string HelpUrl
        {
            get { return "https://gennaroeduardotangarisite.wordpress.com/"; }
        }

        #endregion

        #region Implementation of the IPayPalPlugin interface

        public string DonationDescription
        {
            get { return "paypal description"; }
        }

        public string EmailAccount
        {
            get { return "gennarotangari@msn.com"; }
        }

        #endregion

        #region Implementation of the IGitHubPlugin interface

        public string RepositoryName
        {
            get
            {
                return "GTSolutionImporter";
            }
        }

        public string UserName
        {
            get
            {
                return "getangar";
            }
        }

        #endregion

        #region Constructors
        public GTSolutionImporterControlPlugin()
        {
            InitializeComponent();
        }

        #endregion

        #region Private methods

        private void ImportSolution()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Executing request...",
                Work = (bw, e) =>
                {
                    Type thisType = this.GetType();
                    Type serviceType = this.Service.GetType();

                    String OriginalUrl = (String)(((thisType.GetProperty("ConnectionDetail").GetValue(this, null)).GetType()).GetProperty("OriginalUrl")).GetValue(thisType.GetProperty("ConnectionDetail").GetValue(this, null), null);

                    ClientCredentials credentials = (ClientCredentials)serviceType.GetProperty("ClientCredentials").GetValue(this.Service, null);
                    String username = credentials.UserName.UserName;
                    String password = credentials.UserName.Password;

                    if (username == null && password == null)
                    {
                        username = credentials.Windows.ClientCredential.UserName;
                        password = credentials.Windows.ClientCredential.Password;
                    }

                    String currentOrg = "";

                    try
                    {
                        for (int i = 0; i < lstOrgs.SelectedItems.Count; i++)
                        {
                            System.Collections.Generic.KeyValuePair<string, string> item = (System.Collections.Generic.KeyValuePair<string, string>)lstOrgs.SelectedItems[i];

                            // Now instantiate che PFE Parallel Library and create the requests
                            var serverUri = XrmServiceUriFactory.CreateOrganizationServiceUri(item.Key);
                            OrganizationServiceManager manager = new OrganizationServiceManager(serverUri, username, password);

                            System.Collections.Generic.KeyValuePair<string, string> obj = (System.Collections.Generic.KeyValuePair<string, string>)lstOrgs.SelectedItems[i];
                            byte[] solutionBytes = File.ReadAllBytes(solutionLocation);

                            List<OrganizationRequest> requests = new List<OrganizationRequest>();
                            ImportSolutionRequest importSolutionRequest = new ImportSolutionRequest()
                            {
                                CustomizationFile = solutionBytes,
                                ImportJobId = Guid.NewGuid()
                            };

                            requests.Add(importSolutionRequest);
                            manager.ParallelProxy.Execute(requests);
                            
                            Microsoft.Xrm.Sdk.Messages.RetrieveRequest retrieveRequest = new Microsoft.Xrm.Sdk.Messages.RetrieveRequest();
                            retrieveRequest.ColumnSet = new ColumnSet(new String[] { "data", "solutionname" });
                            retrieveRequest.Target = new EntityReference("importjob", importSolutionRequest.ImportJobId);

                            List<Microsoft.Xrm.Sdk.Messages.RetrieveRequest> retrieveRequests = new List<Microsoft.Xrm.Sdk.Messages.RetrieveRequest>();
                            retrieveRequests.Add(retrieveRequest);

                            IEnumerable<Entity> coll = manager.ParallelProxy.Retrieve(retrieveRequests);

                            if (coll.Count() == 1)
                            {
                                String xmlData = (String)coll.ElementAt(0).Attributes["data"];
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(xmlData);

                                String ImportedSolutionName = doc.SelectSingleNode("//solutionManifest/UniqueName").InnerText;
                                String SolutionImportResult = doc.SelectSingleNode("//solutionManifest/result/@result").Value;

                                if (SolutionImportResult == "success")
                                {
                                    String message = "Solution : " + solutionLocation.Substring(solutionLocation.LastIndexOf(@"\") + 1) + " imported with success in " + obj.Value;
                                    //lstImportStatus.ForeColor = Color.Green;
                                    lstImportStatus.Items.Add(message);

                                    System.Xml.XmlNodeList optionSets = doc.SelectNodes("//optionSets/optionSet");
                                    System.Xml.XmlNodeList entities = doc.SelectNodes("//entities/entity");
                                    System.Xml.XmlNodeList appModules = doc.SelectNodes("//AppModules/AppModule");
                                    System.Xml.XmlNodeList appModuleSiteMaps = doc.SelectNodes("//AppModuleSiteMaps/AppModuleSiteMap");
                                    System.Xml.XmlNodeList nodes = doc.SelectNodes("//nodes/node");
                                    System.Xml.XmlNodeList settings = doc.SelectNodes("//settings/setting");
                                    System.Xml.XmlNodeList interactioncentricdashboards = doc.SelectNodes("//interactioncentricdashboards/interactioncentricdashboard");
                                    System.Xml.XmlNodeList dashboards = doc.SelectNodes("//dashboards/dashboard");
                                    System.Xml.XmlNodeList dialogs = doc.SelectNodes("//dialogs/dialog");
                                    System.Xml.XmlNodeList securityroles = doc.SelectNodes("//securityroles/securityrole");
                                    System.Xml.XmlNodeList workflows = doc.SelectNodes("//workflows/workflow");
                                    System.Xml.XmlNodeList templates = doc.SelectNodes("//templates/template");
                                    System.Xml.XmlNodeList solutionPluginAssemblies = doc.SelectNodes("//SolutionPluginAssemblies/SolutionPluginAssembly");
                                    System.Xml.XmlNodeList sdkMessageProcessingSteps = doc.SelectNodes("//SdkMessageProcessingSteps/SdkMessageProcessingStep");
                                    System.Xml.XmlNodeList serviceEndpoints = doc.SelectNodes("//ServiceEndpoints/ServiceEndpoint");
                                    System.Xml.XmlNodeList webResources = doc.SelectNodes("//webResources/webResource");
                                    System.Xml.XmlNodeList customControls = doc.SelectNodes("//customControls/customControl");
                                    System.Xml.XmlNodeList reports = doc.SelectNodes("//reports/report");
                                    System.Xml.XmlNodeList fieldSecurityProfiles = doc.SelectNodes("//FieldSecurityProfiles/FieldSecurityProfile");
                                    System.Xml.XmlNodeList channelPropertyGroups = doc.SelectNodes("//ChannelPropertyGroups/ChannelPropertyGroup");
                                    System.Xml.XmlNodeList convertrules = doc.SelectNodes("//convertrules/convertrule");
                                    System.Xml.XmlNodeList routingrules = doc.SelectNodes("//routingrules/routingrule");
                                    System.Xml.XmlNodeList slas = doc.SelectNodes("//Slas/Sla");
                                    System.Xml.XmlNodeList channelaccessprofiles = doc.SelectNodes("//channelaccessprofiles/channelaccessprofile");
                                    System.Xml.XmlNodeList profilerules = doc.SelectNodes("//profilerules/profilerule");
                                    System.Xml.XmlNodeList languages = doc.SelectNodes("//languages/language");
                                    System.Xml.XmlNodeList savedQuery = doc.SelectNodes("//entitySubhandlers/savedQuery");
                                    System.Xml.XmlNodeList savedQueryformXml = doc.SelectNodes("//entitySubhandlers/formXml");
                                    System.Xml.XmlNodeList entityRibbon = doc.SelectNodes("//entitySubhandlers/entityRibbon");
                                    System.Xml.XmlNodeList entityCustomResources = doc.SelectNodes("//entitySubhandlers/entityCustomResources");
                                    System.Xml.XmlNodeList hierarchyrule = doc.SelectNodes("//entitySubhandlers/hierarchyrule");
                                    System.Xml.XmlNodeList customControlDefaultConfig = doc.SelectNodes("//entitySubhandlers/CustomControlDefaultConfig");
                                    System.Xml.XmlNodeList savedQueryVisualization = doc.SelectNodes("//entitySubhandlers/savedQueryVisualization");
                                    System.Xml.XmlNodeList publishes = doc.SelectNodes("//publishes/publish");
                                    System.Xml.XmlNodeList rootComponents = doc.SelectNodes("//rootComponents/rootComponent");
                                    System.Xml.XmlNodeList dependencies = doc.SelectNodes("//dependencies/dependency");
                                    System.Xml.XmlNodeList entityrelationships = doc.SelectNodes("//entityrelationships/entityrelationship");

                                    foreach (System.Xml.XmlNode node in optionSets)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in entities)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in appModules)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in appModuleSiteMaps)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in nodes)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in settings)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in interactioncentricdashboards)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in dashboards)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in dialogs)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in securityroles)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in workflows)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in templates)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in solutionPluginAssemblies)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in sdkMessageProcessingSteps)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in serviceEndpoints)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in webResources)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in customControls)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in reports)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);                                            
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in fieldSecurityProfiles)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in channelPropertyGroups)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in convertrules)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in routingrules)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in slas)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in channelaccessprofiles)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in profilerules)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in languages)
                                    {
                                        string OptionSetName = "Language";
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in savedQuery)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in savedQueryformXml)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in entityRibbon)
                                    {
                                        string OptionSetName = node.Attributes["LocalizedName"].Value;
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in entityCustomResources)
                                    {
                                        string OptionSetName = node.Attributes["id"].Value + " - entityCustomResources";
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in hierarchyrule)
                                    {
                                        string OptionSetName = node.Attributes["id"].Value + " - hierarchyrule";
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in customControlDefaultConfig)
                                    {
                                        string OptionSetName = node.Attributes["id"].Value + " - customControlDefaultConfig";
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                            //(lstImportStatus.Items[lstImportStatus.Items.Count - 1]);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in savedQueryVisualization)
                                    {
                                        string OptionSetName = node.Attributes["id"].Value + " - savedQueryVisualization";
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in publishes)
                                    {
                                        string OptionSetName = "Publishes";
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in rootComponents)
                                    {
                                        string OptionSetName = "RootComponents";
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in dependencies)
                                    {
                                        string OptionSetName = "Dependencies";
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    foreach (System.Xml.XmlNode node in entityrelationships)
                                    {
                                        string OptionSetName = "Entityrelationships";
                                        string result = node.FirstChild.Attributes["result"].Value;

                                        if (result == "success")
                                        {
                                            String msg = String.Format("\t{0} result: {1}", OptionSetName, result);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                        else
                                        {
                                            string errorCode = node.FirstChild.Attributes["errorcode"].Value;
                                            string errorText = node.FirstChild.Attributes["errortext"].Value;

                                            String msg = String.Format("\t{0} result: {1} Code: {2} Description: {3}", OptionSetName, result, errorCode, errorText);
                                            lstImportStatus.Items.Add(msg);
                                        }
                                    }

                                    lstImportStatus.Refresh();
                                }
                                else
                                {
                                    String message = "Error while importing the solution : " + solutionLocation.Substring(solutionLocation.LastIndexOf(@"\") + 1) + " in " + obj.Value;
                                    lstImportStatus.Items.Add(message);
                                }                                
                            }
                        }
                    }
                    catch(Exception exc)
                    {
                        String message = "Error while importing the solution " + solutionLocation.Substring(solutionLocation.LastIndexOf(@"\") + 1) + " in " + currentOrg;
                        lstImportStatus.Items.Add(message);
                    }                   
                },
                PostWorkCallBack = e =>
                {
                    if (e.Error == null)
                    {
                        MessageBox.Show("Operation completed successfully", "GT Solution Importer");
                        //RetrieveRecordFromFetchXml();
                    }
                    else
                    {
                        MessageBox.Show(e.Error.Message, "GT Solution Importer");
                    }
                }
            });
        }

        #endregion

        #region Toolstrip buttons

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool(); // PluginBaseControl method that notifies the XrmToolBox that the user wants to close the plugin
            // Override the ClosingPlugin method to allow for any plugin specific closing logic to be performed (saving configs, canceling close, etc...)
        }

        private void tsbAbout_Click(object ender, EventArgs e)
        {

            MessageBox.Show("GT Solution Importer - Version " + typeof(GTSolutionImporter).Assembly.GetName().Version + "\n\n(c)Coypright by Gennaro Eduardo Tangari", "GT Solution Importer");
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            CancelWorker(); // PluginBaseControl method that calls the Background Workers CancelAsync method.

            MessageBox.Show("Cancelled");
        }

        private void tsbClear_Click(object sender, EventArgs e)
        {
            lstOrgs.DataSource = null;
            lstImportStatus.Items.Clear();
            
            txtSolutionPath.Text = "";
            lblImportSolution.Text = "No Solution have been imported";
            lblSolution.Text = "Solution not selected";

            isSolutionSelected = false;
            solutionLocation = String.Empty;
        }

        #endregion

        #region Interface

        private void btnGetOrgs_Click(object sender, EventArgs e)
        {
            Type thisType = this.GetType();
            Type serviceType = this.Service.GetType();

            String OrganizationServiceUrl = (String)(((thisType.GetProperty("ConnectionDetail").GetValue(this, null)).GetType()).GetProperty("OrganizationServiceUrl")).GetValue(thisType.GetProperty("ConnectionDetail").GetValue(this, null), null);
            String DiscoveryServiceUrl = OrganizationServiceUrl.Substring(0, OrganizationServiceUrl.LastIndexOf('/')) + "/Discovery.svc";
            Boolean IsCrmOnline = (Boolean)((((thisType.GetProperty("ConnectionDetail").GetValue(this, null)).GetType()).GetProperty("UseOnline")).GetValue(thisType.GetProperty("ConnectionDetail").GetValue(this, null), null));

            if (IsCrmOnline)
            {
                MessageBox.Show("GT Solution Importer currently support only On-Premise deployment only!", "GT Solution Importer");
                lstOrgs.DataSource = null;

                return;
            }

            ClientCredentials credentials = (ClientCredentials)serviceType.GetProperty("ClientCredentials").GetValue(this.Service, null);
            
            using (var discoveryProxy = new DiscoveryServiceProxy(new Uri(DiscoveryServiceUrl), null, credentials, null))
            {
                discoveryProxy.Authenticate();

                // Get all Organizations using Discovery Service

                RetrieveOrganizationsRequest retrieveOrganizationsRequest =
                new RetrieveOrganizationsRequest()
                {
                    AccessType = EndpointAccessType.Default,
                    Release = OrganizationRelease.Current
                };

                RetrieveOrganizationsResponse retrieveOrganizationsResponse =
                (RetrieveOrganizationsResponse)discoveryProxy.Execute(retrieveOrganizationsRequest);
               
                if (retrieveOrganizationsResponse.Details.Count > 0)
                {
                    Dictionary<string, string> collections = new Dictionary<string, string>();

                    foreach (OrganizationDetail orgInfo in retrieveOrganizationsResponse.Details)
                    {                        
                        String urlName = orgInfo.Endpoints.Where(org => org.Key == EndpointType.OrganizationService).FirstOrDefault().Value;
                        collections.Add(urlName, orgInfo.FriendlyName);
                    }


                    lstOrgs.DataSource = new BindingSource(collections, null);
                    lstOrgs.DisplayMember = "Value";
                    lstOrgs.ValueMember = "Key";
                    lstOrgs.SelectedIndex = 0;
                }
            }
        }

        private void btnSelectSolution_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "GT Solution Importer";
            openFile.Filter = "ZIP|*.zip";
            openFile.Multiselect = false;
            openFile.ShowDialog();

            if (openFile.FileName != "")
            {
                txtSolutionPath.Text = openFile.FileName;
                lblSolution.Text = "Solution to import: " + openFile.FileName.Substring(openFile.FileName.LastIndexOf(@"\") + 1);

                isSolutionSelected = true;
                solutionLocation = openFile.FileName;
            }
            else
            {
                isSolutionSelected = false;
                lblSolution.Text = "Solution not selected";

            }
        }

        private void btnImportSolution_Click(object sender, EventArgs e)
        {
            if (lstOrgs.Items.Count > 0 && lstOrgs.SelectedItems.Count > 0)
            {
                if (isSolutionSelected)
                {
                    if (System.IO.File.Exists(txtSolutionPath.Text))
                    {
                        if (System.IO.Path.GetExtension(txtSolutionPath.Text).ToUpper() == ".ZIP")
                        {
                            ExecuteMethod(ImportSolution);
                        }
                        else
                        {
                            MessageBox.Show("Before to proceed please select a valid solution file!", "GT Solution Importer");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Before to proceed please select a valid solution file!", "GT Solution Importer");
                }
            }
            else
            {
                MessageBox.Show("Before to proceed please select at least one target organization!", "GT Solution Importer");
            }
        }


        #endregion

        #region Custom events

        private void lstImportStatus_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            Brush currentBrush = Brushes.Green;

            e.DrawBackground();
          
            if (!(sender as ListBox).Items[e.Index].ToString().Contains("success"))
            {
                currentBrush = Brushes.Red;
            }

            Rectangle bounds = e.Bounds;
            bounds.Height += 10;

           // e.Graphics.DrawString(lstImportStatus.Items[e.Index].ToString(), e.Font, currentBrush, bounds, StringFormat.GenericDefault);
            e.Graphics.DrawString(lstImportStatus.Items[e.Index].ToString(), Font, currentBrush, bounds);

            e.DrawFocusRectangle();
        }

        #endregion
    }
}