using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using SubscriptionUtility;

namespace SubscriptionEncryption
{
    class Program
    {
        static void Main(string[] args)
        {
            string crypticFilePath = @"C:\temp\cryptic.txt";
            SubscriptionChecker subscriptionChecker = new SubscriptionChecker();
            string subscriptions = subscriptionChecker.GetSubscriptions(42);

            // Encrypt the data.
            string cryptic = Security.Encode(subscriptions);
            Security.WriteToFile(crypticFilePath, cryptic);

            // Decrypt the data.
            string encryptedData = Security.ReadFromFile(crypticFilePath);
            string decrypted = Security.Decode(encryptedData);

            // Parse the xml into a list of objects
            XmlSerializer serializer = new XmlSerializer(typeof(List<Subscription>), new XmlRootAttribute("Subscriptions"));
            System.IO.StringReader xmlFile = new System.IO.StringReader(decrypted);
            List<Subscription> parsedSubscriptions = (List<Subscription>)serializer.Deserialize(xmlFile);

            // Print the data.
            foreach( Subscription sub in parsedSubscriptions)
            {
                if (sub.IsSubscribed)
                    Console.WriteLine(sub.Name + " is subscribed.");
                else
                    Console.WriteLine(sub.Name + " is not subscribed.");
            }
            
            Console.ReadKey();
        }
    }
}
