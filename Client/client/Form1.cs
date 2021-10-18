using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


/*
       Doruk SOYPACACI          25506
       Ozge ONEYMAN             24906
       Mert SASMAZ              24290
       Muhammed Batuhan ODABASI 23636
*/


namespace client
{
    public partial class username_label : Form
    {
        string shorfilename = "";

        bool terminating = false;
        bool connected = false;
        Socket clientSocket;

        string path;

        string username;
        List<int> filesize = new List<int>();
        int numberatlist = 0;

        public username_label()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        int semaphore = 0;


        //a mutex for file send
        int packet_mutex = 0;


        //PATH for downloading files.
        string path_for_dowload = "";


        //a verification for download alg

        string download_code = "stop";
 
        //---------------------------------------------------------------------------------------------------------
        //--------------------------------------BUTTON CONNECT---------------------------------------------------
        //---------------------------------------------------------------------------------------------------------



        private void button_connect_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = textBox_ip.Text;

            int portNum;
            if(Int32.TryParse(textBox_port.Text, out portNum))
            {
                try
                {
                    //Takes username from the username box.
                     username = username_box.Text;

                    //Check if the username is valid or not. If not valid print a msg to box for informing the user to choosing a valid usrname.
                     if (username != "" && username.Length <= 64)
                     {
                         //Socket created between server.
                         clientSocket.Connect(IP, portNum);


                         //Enabled the buttons
                         browse_button.Enabled = true;
                         Upload_button.Enabled = true;
                         delete_button.Enabled = true;
                         Copy_button.Enabled = true;
                         disconnect_button.Enabled = true;
                         change_to_public.Enabled = true;
                         browse_for_download.Enabled = true;

                         //Send the username to server to server to check if it is used.
                         Byte[] buffer = new Byte[1024];
                         buffer = Encoding.Default.GetBytes(username);
                         clientSocket.Send(buffer);

                         //Connects the server but before any permission given from the server (about username's uniqueness**) client cannot upload any file.
                         connected = true;
                         logs.AppendText("Connected to the server!\n");




                         button_connect.Enabled = false;
                         Thread receiveThread = new Thread(Receive);
                         receiveThread.Start();
                     }
                     else
                         logs.AppendText("Enter a valid username!!!\n");


                }
                catch
                {
                    logs.AppendText("Could not connect to the server!\n");
                    
                }
            }
            else
            {
                logs.AppendText("Check the port\n");
     
            }

        }

        //---------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------




        //------------------------------------------------------------------------------------------------------------
        //--------------------------------RECEIVE---------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------



        private void Receive()
        {
            while(connected)
            {
                try
                {
                    Byte[] buffer = new Byte[1024];
                    clientSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));



                   //This string is determined as an error code. If any client personally recieves it after its attempt of connection; Client disconnect itself.
                   string error_message = "Username is already at use!!!";
                    if (incomingMessage == error_message)
                    {
                        //error message 
                            logs.AppendText("Server: " + incomingMessage + "\n");

                            clientSocket.Close();
                            connected = false;
                            logs.AppendText("Chose another username. Disconnected from server.\n");
                            button_connect.Enabled = true;

                            

                    }
                    if (incomingMessage == "G3TAFILE")
                    {

                        //WRITE INTO FILE

                        Byte[] buffer_message = new Byte[1024];
                        clientSocket.Receive(buffer_message);

                        string incomingMessage2 = Encoding.Default.GetString(buffer_message);
                        incomingMessage2 = incomingMessage2.Substring(0, incomingMessage2.IndexOf("\0"));


                        //write into a new txt
                        

                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(path_for_dowload + @"\" + username + "--database_of_file_for_client" + @".txt", true))
                        {
                          logs.AppendText(incomingMessage2 + "\n");
                            sw.WriteLine(incomingMessage2 + "\n");
                            numberatlist += 1;
                        }
                    }


                    //-------------Download algorithm feedback

                    if (incomingMessage == "Download_is_valid")
                    {
                        download_code = "go";
                    }


                    //----------------a mutex activator
                    if (incomingMessage == "continue")
                    {

                        packet_mutex--;

                    }

                    // whole recieve file code from server
                    //-----------------------------------------------------------------------------------------------
                    //-----------------------------------------------------------------------------------------------

                    if (incomingMessage == "P4CK3TR3C13V3")
                    {

                        string Message = "C0NT1NU3";

                        byte[] buffer271 = Encoding.Default.GetBytes(Message);
                        clientSocket.Send(buffer271);
                        
                        
                        //The next message recieving--------------------------------------------
                        Byte[] buffer_message2 = new Byte[1024];
                        clientSocket.Receive(buffer_message2);

                        //The count of packets are received.
                        string incomingMessage_packet_size = Encoding.Default.GetString(buffer_message2);
                        incomingMessage_packet_size = incomingMessage_packet_size.Substring(0, incomingMessage_packet_size.IndexOf("\0"));
                        //------------------------------------------------------------------------------------------------------

                        int packcount = Int32.Parse(incomingMessage_packet_size); //packet size changed to int


                        //a feedback to continue---------------------------------------------
                        byte[] buffer273 = Encoding.Default.GetBytes(Message);

                            clientSocket.Send(buffer273);
                        
 
                        //----------------------------------------------------------------------

                        //-------------------------------------------------file name recieve---------

                        Byte[] buffer_message24342 = new Byte[1024];
                        clientSocket.Receive(buffer_message24342);

                        //The count of packets are received.
                        string the_name_o_file = Encoding.Default.GetString(buffer_message24342);
                        the_name_o_file = the_name_o_file.Substring(0, the_name_o_file.IndexOf("\0"));
                        //------------------------------------------------------------------------------------------------------


                        string fileshortname = the_name_o_file; //packet size changed to int


                        //-------------------------------------------------------------------------

                        string fileName = path_for_dowload + @"\" + fileshortname + @".txt";
                        try
                        {

                            // Check if file already exists. If yes, change the name of the file------------------- CIAE

                            int filename_counter = 1;
                            //counts the replicants of files' counts.
                            string filename_counter_s = "";
                            string fileName_for_loop = fileName;

                            string fileshortname_actual = fileshortname;

                            while (File.Exists(fileName_for_loop))
                            {
                                // must add (n) if file exists.
                                filename_counter_s = filename_counter.ToString();
                                fileshortname = fileshortname_actual + "(" + filename_counter_s + ")";

                                fileName_for_loop = path_for_dowload + @"\" + fileshortname + @".txt";
                                filename_counter++;

                            }
                            filename_counter = 1;



                            //a feedback to continue---------------------------------------------
                            byte[] buffer2371 = Encoding.Default.GetBytes(Message);
                                clientSocket.Send(buffer2371);

                            //----------------------------------------------------------------------

                            //-------------------------------------------------------name received a file created.


                            //--------------------------------------each line receiving-----------------------------

                            for (int i = 0; i < packcount; i++)
                            {

                                //The next message recieving--------------------------------------------
                                Byte[] buffer_file_msg = new Byte[1024];
                                clientSocket.Receive(buffer_file_msg);

                                //The count of packets are received.
                                string one_o_line = Encoding.Default.GetString(buffer_file_msg);

                                if (one_o_line[1023] == '\0')
                                    one_o_line = one_o_line.Substring(0, one_o_line.IndexOf("\0"));
                                //------------------------------------------------------------------------------------------------------

                                string one_line = one_o_line;

                                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(path_for_dowload + @"\" + fileshortname + @".txt", true))
                                {
                                    sw.Write(@one_line);
                                }
                                // Write file contents on file.     

                                //a feedback to continue---------------------------------------------
                                byte[] buffer24731 = Encoding.Default.GetBytes(Message);
                                    clientSocket.Send(buffer24731);
                       
                                //----------------------------------------------------------------------

                            }

                            //----------------a notification printed-----------------------------

                            //server prints the a message that it receive a file from client.
                            logs.AppendText("An incoming file received called " + fileshortname + "\n");


                        }
                        catch (Exception Ex)
                        {
                            Console.WriteLine(Ex.ToString());
                        }






                    }

                    //-----------------------------------------------------------------------------------------------
                    //-----------------------------------------------------------------------------------------------
                    //-----------------------------------------------------------------------------------------------
                    //-----------------------------------------------------------------------------------------------









                   

                }
                catch
                {
                    if (!terminating)
                    {
                        logs.AppendText("The server has disconnected\n");
                        button_connect.Enabled = true;

                    }

                    clientSocket.Close();
                    connected = false;

                }

            }
        }



        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------




        //---------------------------------------------------------------------------------------------------------
        //-----------------------------------------FORM CLOSING----------------------------------------------------
        //---------------------------------------------------------------------------------------------------------


        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }

        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------








        //------------------------------------------------------------------------------------------------------------
        //----------------------------------BROWSE BUTTON CLICK-------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        private void browse_button_Click(object sender, EventArgs e)
        {

            Upload_button.Enabled = true;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            
            //Select a file from the computer in order to send the server.

            openFileDialog1.InitialDirectory = "C://Desktop";
            openFileDialog1.Title = "Select file to be upload.";
            openFileDialog1.Filter = "Select Valid Document(*.txt)|*.txt";
            openFileDialog1.FilterIndex = 1;
            try
            {
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (openFileDialog1.CheckFileExists)
                    {
                        path = System.IO.Path.GetFullPath(openFileDialog1.FileName);

                        shorfilename = Path.GetFileName(path);
                      
                        //print the file name and to label directory text.
                        label_directory.Text = path;

                    }
                }
                else
                {
                    MessageBox.Show("Please choose a document.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------









        //------------------------------------------------------------------------------------------------------------
        //-----------------------------------UPLOAD BUTTON-----------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        private void Upload_button_Click(object sender, EventArgs e)
        {

            packet_mutex = 0;

            //-------- a counter for each 20 lines will be also number of packs.
            int pack_count;

            //Read all the lines from the file whose path preiously determined, then put each line in a string vector. 
            string[] lines = System.IO.File.ReadAllLines(path);

            //--------------------------------------------------------------------------------


            //Real name finder kit-----------------------------------RNFK
            //--These part finds the actual name of file (without its .txt)

            int ls = shorfilename.Length;
            ls = ls - 4;


            //short file name without txt
            string real_short_file_name = "";

            for (int i = 0; i < ls; i++)
                real_short_file_name += shorfilename[i];

            //-------------------------------------------------------RNFK

            //--------------firstly must send the code for file sending--------------------

            string send_file_code = "P4CK3TR3C13V3";

            byte[] LogoDataBy = ASCIIEncoding.ASCII.GetBytes(send_file_code);
            clientSocket.Send(LogoDataBy);

            packet_mutex++;

            //------------------------------------------------------------------------------



            while (packet_mutex != 0)
            { }

            //----------------------------------------------find the count of packs and send it-------------

     

            int demo_counter = 0;
            char demo_string;


            List<char> demo_char = new List<char>();

            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    int demo_i = sr.Read();

                    if (demo_i != 13)
                    {

                        demo_counter++;

                    }
                }
            }

            string cs = demo_counter.ToString();


            if (demo_counter % 1024 == 0)
                pack_count = demo_counter / 1024;
            else
                pack_count = (demo_counter / 1024) + 1;



            string packs_c = pack_count.ToString();


            byte[] LogoDataBy_pack = ASCIIEncoding.ASCII.GetBytes(packs_c);

            clientSocket.Send(LogoDataBy_pack);
            packet_mutex++;

            //--------------------------------------------------



            //SLEEP COMMAND
            while (packet_mutex != 0)
            { }


            //--------------------------------------------file name sending--------------------------------------

            string filenamesent = username + "_" + real_short_file_name;

            byte[] files_name = ASCIIEncoding.ASCII.GetBytes(filenamesent);

            clientSocket.Send(files_name);
            packet_mutex++;



            //SLEEP COMMAND and mutex
            while (packet_mutex != 0)
            { }

            //---------------------------------------------------------------------------------------------------

            //Prints message of the files size
            logs.AppendText("File called " + filenamesent + ": " + demo_counter + " bytes is sent" + "\n");




            //------------------------------------file's send-------------------------------------------------

            int some_needed_counter = 0;

            string the_message = "";
            int ccount = 0;
            

                using (StreamReader sr = new StreamReader(path))
                {
                    while (sr.Peek() >= 0)
                    {
                        
                        int demo_i = sr.Read();

                        if (demo_i != 13)
                        {
                            demo_string = (char)demo_i;

                            the_message += demo_string;

                            some_needed_counter++;

                            if (some_needed_counter == 1024)
                            {
                                //send cmnd

                                some_needed_counter = 0;

                                byte[] msg_byte = ASCIIEncoding.ASCII.GetBytes(the_message);

                                clientSocket.Send(msg_byte);
                                packet_mutex++;


                                while (packet_mutex != 0)
                                { }

                                the_message = "";
                                //ccount++;

                              //  if (pack_count <= ccount)
                                   // break;

                            }


                        }
                    }
                }

                if (pack_count % 1024 != 0)
                {
                    byte[] msg_byte = ASCIIEncoding.ASCII.GetBytes(the_message);

                    clientSocket.Send(msg_byte);
                    packet_mutex++;


                    while (packet_mutex != 0)
                    { }
                }   
                







        }



        

        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------





        //------------------------------------------------------------------------------------------------------------
        //-----------------------------------DISCONNECT BUTTON-----------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        private void disconnect_button_Click(object sender, EventArgs e)
        {
            clientSocket.Close();
            connected = false;
            logs.AppendText("Disconneting from sever.....\n");
            button_connect.Enabled = true;

            browse_button.Enabled = false;
            Upload_button.Enabled = false;
            Copy_button.Enabled = false;
            delete_button.Enabled = false;
            Copy_button.Enabled = false;
            change_to_public.Enabled = false;
            browse_for_download.Enabled = false;

        }


        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------


        //------------------------------------------------------------------------------------------------------------
        //--------------------------------------COPY BUTTON-----------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------




        private void Copy_button_Click(object sender, EventArgs e)
        {

            string dub_file_name = message_box.Text;

            //took the coppied file name and send the coppied code and name as two messages to server. 

            dub_file_name = username + "_" +dub_file_name;

            string copy_file_code = "DUBM3SS4GER3C13V3";

            //-----------------------copy code --------------------------

            Byte[] buffer1 = new Byte[1024];
            buffer1 = Encoding.Default.GetBytes(copy_file_code);
            clientSocket.Send(buffer1);

            //-------------------------------------------------------------

            System.Threading.Thread.Sleep(3000);


            //-------------------------copy file name----------------------
            Byte[] buffer2 = new Byte[1024];
            buffer2 = Encoding.Default.GetBytes(dub_file_name);
            clientSocket.Send(buffer2);
            //-------------------------------------------------------------


            /* old send code
                Byte[] buffer = new Byte[1024];
                buffer = Encoding.Default.GetBytes(message_whole);
                clientSocket.Send(buffer);
             
             
             */
            packet_mutex++;



        }

        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------





        //------------------------------------------------------------------------------------------------------------
        //------------------------------------DELETE BUTTON-----------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------



        private void delete_button_Click(object sender, EventArgs e)
        {
            string dub_file_name = message_box.Text;

            //took the coppied file name and send the coppied code and name as two messages to server. 

            dub_file_name = username + "_" + dub_file_name;

            string copy_file_code = "D3L3T10N0FF1L3";

            //-----------------------copy code --------------------------

            Byte[] buffer1 = new Byte[1024];
            buffer1 = Encoding.Default.GetBytes(copy_file_code);
            clientSocket.Send(buffer1);

            //-------------------------------------------------------------

            System.Threading.Thread.Sleep(3000);


            //-------------------------copy file name----------------------
            Byte[] buffer2 = new Byte[1024];
            buffer2 = Encoding.Default.GetBytes(dub_file_name);
            clientSocket.Send(buffer2);
            //-------------------------------------------------------------

            packet_mutex++;

            /* old send code
                Byte[] buffer = new Byte[1024];
                buffer = Encoding.Default.GetBytes(message_whole);
                clientSocket.Send(buffer);
             
             
             */

        }

        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------





        //------------------------------------------------------------------------------------------------------------
        //-----------------------------------------DOWNLOAD BUTTON----------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        private void Download_button_Click(object sender, EventArgs e)
        {
            string dub_file_name = message_box.Text;

            //took the coppied file name and send the coppied code and name as two messages to server. 



            string copy_file_code = "D0WNL04DF1L3";

            //-----------------------DOWLOAD code --------------------------

            Byte[] buffer1 = new Byte[1024];
            buffer1 = Encoding.Default.GetBytes(copy_file_code);
            clientSocket.Send(buffer1);

            //-------------------------------------------------------------





                //-------------------------DOWLOAD file name----------------------
                Byte[] buffer2 = new Byte[1024];
                buffer2 = Encoding.Default.GetBytes(dub_file_name);
                clientSocket.Send(buffer2);
                //-------------------------------------------------------------


            System.Threading.Thread.Sleep(2000);
            packet_mutex++;


            if (download_code != "go")
            {
                logs.AppendText("A false filename has been choosen for download." + "\n");

            }
         
                    
        }




        //------------------------------------------------------------------------------------------------------------
        //-----------------------------------------BROWSE FOR DOWNLOAD------------------------------------------------
        //------------------------------------------------------------------------------------------------------------


        private void browse_for_download_Click(object sender, EventArgs e)
        {
            OpenFileDialog folderBrowser = new OpenFileDialog();

            //select a file in order to use for keeping our receive files and database.txt file.

            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;

            folderBrowser.FileName = "Folder Selection.";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                path_for_dowload = Path.GetDirectoryName(folderBrowser.FileName);
                logs.AppendText("Path: " + path_for_dowload + "\n");


                Get_a_File.Enabled = true;
                Download_button.Enabled = true;
            }

            

        }

        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------




        //------------------------------------------------------------------------------------------------------------
        //------------------------------------GET OWNED FILES--------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        private void Get_a_File_Click(object sender, EventArgs e)
        {
            string get_file_code = "G3TAFILE";
            string username = username_box.Text;

            Byte[] buffer = new Byte[1024];
            buffer = Encoding.Default.GetBytes(get_file_code);

            clientSocket.Send(buffer);

            logs.AppendText("DOWNLOADABLE FILES FROM SERVER ARE:" + "\n");


            System.Threading.Thread.Sleep(300);

            Byte[] buffer2 = new Byte[1024];
            buffer2 = Encoding.Default.GetBytes(username);
            clientSocket.Send(buffer2);

            packet_mutex++;
        }
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------




        //------------------------------------------------------------------------------------------------------------
        //---------------------------------------CHANGE TO PUBLIC-----------------------------------------------------
        //------------------------------------------------------------------------------------------------------------


        private void change_to_public_Click(object sender, EventArgs e)
        {
            string dub_file_name = message_box.Text;

            //took the coppied file name and send the coppied code and name as two messages to server. 

            dub_file_name = username + "_" + dub_file_name;

            string copy_file_code = "CH4NG3T0PUBL1C";

            //-----------------------copy code --------------------------

            Byte[] buffer1 = new Byte[1024];
            buffer1 = Encoding.Default.GetBytes(copy_file_code);
            clientSocket.Send(buffer1);

            //-------------------------------------------------------------

            System.Threading.Thread.Sleep(3000);


            //-------------------------copy file name----------------------
            Byte[] buffer2 = new Byte[1024];
            buffer2 = Encoding.Default.GetBytes(dub_file_name);
            clientSocket.Send(buffer2);
            //-------------------------------------------------------------

            packet_mutex++;

            /* old send code
                Byte[] buffer = new Byte[1024];
                buffer = Encoding.Default.GetBytes(message_whole);
                clientSocket.Send(buffer);
             
             
             */
        }



        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------


    }
}
