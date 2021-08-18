using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TeleSharpUWP.TL;
using TeleSharpUWP.TL.Contacts;
//using TeleSharp.TL;
//using TLSharp.Core;
//using TLSharp.Core.Exceptions;
using TLSharpUWP;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace Telega
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //App api_id = 7777870
        //api_hash=d35366d27895f506b459aca6ecf16abc
        public MainPage()
        {
            this.InitializeComponent();
           // TLSharp.Core.FileSessionStore store = new TLSharp.Core.FileSessionStore();
           // client = new TelegramClient(apiId, apiHash, store, "session");


        }
        string nomer = "79152142851";
        int apiId = 7777870;
        String apiHash = "d35366d27895f506b459aca6ecf16abc";
        TelegramClient client;
        string hash;
        private async void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              
                TLSharpUWP.FileSessionStore store = new TLSharpUWP.FileSessionStore();
                
                 client = new TelegramClient(apiId, apiHash, store, "session");
                //if your app is not autenticated by telegram this code will send  request to add a new device then telegram will sent you a Autenticatin code
                await client.ConnectAsync();
                
            }
            catch(Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
                //hash = await client.SendCodeRequestAsync(nomer);
            }

        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                //if your app is not autenticated by telegram and you have sent request to telegram, this code will add new device using autenticatin code that telegram sent to you
                var code = txtAutCode.Text;  // this is a TextBox that you must insert the code that Telegram sent to you
                var user = await client.MakeAuthAsync(nomer, hash, code);
            }
            catch(Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
            }
        }

        private async void button2_Click(object sender, RoutedEventArgs e)
        {

            await client.ConnectAsync();

            // Here is the code that will add a new contact and send a test message
            TLRequestImportContacts requestImportContacts = new TLRequestImportContacts();
            requestImportContacts.Contacts = new TLVector<TLInputPhoneContact>();
            requestImportContacts.Contacts.Add(new TLInputPhoneContact()
            {
                Phone = "new Number in global format example : 98916*******",
                FirstName = "new Number's FirstName",
                LastName = "new Number's LastName"
            });
            var o = await client.SendRequestAsync<TLImportedContacts>((TLMethod)requestImportContacts);
            var NewUserId = (o.Users.First() as TLUser).Id;
            var d = await client.SendMessageAsync(new TLInputPeerUser() { UserId = NewUserId }, "test message text");



            //find recipient in contacts and send a message to it
            var result = await client.GetContactsAsync();
            var user = result.Users
                .Where(x => x.GetType() == typeof(TLUser))
                .Cast<TLUser>()
                .FirstOrDefault(x => x.Phone == "recipient Number in global format example : 98916*******");
            MessageDialog messageDialog = new MessageDialog((user.LastName).ToString());
            await messageDialog.ShowAsync();
            //MessageBox.Show((user.LastName).ToString());

            //send message
            await client.SendMessageAsync(new TLInputPeerUser() { UserId = user.Id }, "hi test message");
        }
        public async void getContact()
        {

            var result = await client.GetContactsAsync();


            string ss1 = String.Empty;


            foreach (var d in result.Contacts)
            {
                var user = result.Users
              .Where(x => x.GetType() == typeof(TLUser))
              .Cast<TLUser>()
              .FirstOrDefault(x => x.Id == d.UserId);
                if (user != null)
                {
                    try
                    {
                        if (user.FirstName != null && user.LastName != null)
                        {
                            ss1 += (user.FirstName).ToString() + "\t" + (user.LastName).ToString() + "\n";
                        }
                        else
                        {
                            if (user.FirstName != null)
                            {
                                try
                                {


                                    ss1 += (user.FirstName).ToString() + "\n";
                                }
                                catch (Exception)
                                {

                                }
                            }
                            if (user.LastName != null)
                            {
                                try
                                {


                                    ss1 += (user.LastName).ToString() + "\n";
                                }
                                catch (Exception)
                                {

                                }
                            }

                        }

                    }
                    catch (Exception)
                    {

                    }
                }




            }
           

        }
    }
    
}
/*149.154.167.40:443
 * -----BEGIN RSA PUBLIC KEY-----
MIIBCgKCAQEAyMEdY1aR+sCR3ZSJrtztKTKqigvO/vBfqACJLZtS7QMgCGXJ6XIR
yy7mx66W0/sOFa7/1mAZtEoIokDP3ShoqF4fVNb6XeqgQfaUHd8wJpDWHcR2OFwv
plUUI1PLTktZ9uW2WE23b+ixNwJjJGwBDJPQEQFBE+vfmH0JP503wr5INS1poWg/
j25sIWeYPHYeOrFp/eXaqhISP6G+q2IeTaWTXpwZj4LzXq5YOpk4bYEQ6mvRq7D1
aHWfYmlEGepfaYR8Q0YqvvhYtMte3ITnuSJs171+GDqpdKcSwHnd6FudwGO4pcCO
j4WcDuXc2CTHgH8gFTNhp/Y8/SpDOhvn9QIDAQAB
-----END RSA PUBLIC KEY-----
149.154.167.50:443
-----BEGIN RSA PUBLIC KEY-----
MIIBCgKCAQEA6LszBcC1LGzyr992NzE0ieY+BSaOW622Aa9Bd4ZHLl+TuFQ4lo4g
5nKaMBwK/BIb9xUfg0Q29/2mgIR6Zr9krM7HjuIcCzFvDtr+L0GQjae9H0pRB2OO
62cECs5HKhT5DZ98K33vmWiLowc621dQuwKWSQKjWf50XYFw42h21P2KXUGyp2y/
+aEyZ+uVgLLQbRA1dEjSDZ2iGRy12Mk5gpYc397aYp438fsJoHIgJ2lgMv5h7WY9
t6N/byY9Nw9p21Og3AoXSL2q/2IJ1WRUhebgAdGVMlV1fkuOQoEzR7EdpqtQD9Cs
5+bfo3Nhmcyvk5ftB0WkJ9z6bNZ7yxrP8wIDAQAB
-----END RSA PUBLIC KEY-----
*/