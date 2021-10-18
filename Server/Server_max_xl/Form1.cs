using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
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




namespace server
{
    public partial class Form1 : Form
    {

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clientSockets = new List<Socket>();

        bool terminating = false;
        bool listening = false;

        int serverPort;


        //A stremwriter created to write and read file.
        StreamWriter sw;

        //IncomingMessage is recieved string object.
        string incomingMessage;

        //path of the database file.
        string fileName;

        //path of the folder which database founds in.
        string folderPath;

        int semaphore = 0;

        // A string to determine the username.
        string username = "";


        //A class created for the incomming clients. It has 2 objects socket and username.
          class Client 
          {
            public Socket socket;
            public string username;
            public int packet_mutex = 0;
          }

        //A list which keeps the clients
           List <Client> Client_List = new List<Client>();


        //path of all files found
           string the_path;


        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        int filename_counter = 1;



        //------------------------------------------------------------------------------------------------------------
        //-----------------------------------------BUTTON LISTEN------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------


        private void button_listen_Click(object sender, EventArgs e)
        {

            if(Int32.TryParse(textBox_port.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(300);

                listening = true;
                button_listen.Enabled = false;

                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

                logs.AppendText("Started listening on port: " + serverPort + "\n");

            }
            else
            {
                logs.AppendText("Please check port number \n");
            }
        }

        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------









        //------------------------------------------------------------------------------------------------------------
        //----------------------------------------------ACCEPT--------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------



        private void Accept()
        {
            while(listening)
            {
                try
                {
                    Socket newClient = serverSocket.Accept();


                    ///--------------------------------------USERNAME CHECK-----------------------------------
                    ///----------------------------------------------------------------------------------------
                    ///--------------------------------------------------------------------------------------
                    ///----------A username check if the any online user has the recently sent username. If so the server sends a error msg
                    ///----------than the user disc needs to change its username and this 

                    Byte[] buffer = new Byte[1024];
                    newClient.Receive(buffer);


                    incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));






                    
                    int minus_one_of_clients_count = Client_List.Count();

                    int counter = 0;

                    //If there is no client online in other words if the list is empty then, then it enters this part. This part has no username check!!!
                    if (Client_List.Count() == 0)
                    {

                        string[] lines = System.IO.File.ReadAllLines(fileName);

                        int write_if_this_var_is_zero = 0;

                        //WRITE THE USERNAME IF THE USERNAME DO NOT EXIST IN DATABASE.TXT
                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (incomingMessage == lines[i])
                            {
                                write_if_this_var_is_zero++;
                            }
                            if (write_if_this_var_is_zero > 0)
                                break;
                        }

                        if (write_if_this_var_is_zero == 0)
                        {
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(folderPath + @"\database.txt", true))
                            {
                                sw.WriteLine(incomingMessage);

                            }
                        }

                        // print the client joined.
                        logs.AppendText("Client called  " + incomingMessage + " joined.\n");


                        Client the_Client = new Client();

                        the_Client.socket = (newClient);

                        the_Client.username = (incomingMessage);

                        Client_List.Add(the_Client);


                        Thread receiveThread = new Thread(() => Receive(the_Client)); 
                        receiveThread.Start();

                    }
                    else
                    {

                        //check if the user name exist.
                        foreach (Client c in Client_List)
                        {
                            counter++;

                            if (c.username.Equals(incomingMessage))
                            {
                                logs.AppendText(incomingMessage + " already exists! \n");


                                string Message = "Username is already at use!!!";

                                Byte[] buffer27 = Encoding.Default.GetBytes(Message);



                                try
                                {
                                    newClient.Send(buffer27);

                                    newClient.Close();

                                  


                                }
                                catch
                                {
                                    logs.AppendText("There is a problem! Check the connection...\n");
                                    terminating = true;
                                    textBox_port.Enabled = true;
                                    button_listen.Enabled = true;
                                    serverSocket.Close();
                                }

                                break;
                            }



                                //username accepted.
                            else if ((c.username != incomingMessage) && (minus_one_of_clients_count == counter))// If it  did not entered previous if : it means the client can join the server.// if its the last loop
                            {

                                string[] lines = System.IO.File.ReadAllLines(fileName);

                                int write_if_this_var_is_zero = 0;

                                //WRITE THE USERNAME IF THE USERNAME DO NOT EXIST IN DATABASE.TXT
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    if (incomingMessage == lines[i])
                                    {
                                        write_if_this_var_is_zero++;
                                    }
                                    if (write_if_this_var_is_zero > 0)
                                        break;
                                }

                                //If the write_if_this_var_is_zero = 0 it means there is no record of new client's username. Thus it writes it down to txt file.
                                if (write_if_this_var_is_zero == 0)
                                {
                                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(folderPath + @"\database.txt", true))
                                    {
                                        sw.WriteLine(incomingMessage);

                                    }
                                }


                                // print the client joined.
                                logs.AppendText("Client called  " + incomingMessage + " joined.\n");



                                //----------------A new Client created according to the information recieved in accept function. Then, this newly created client is added to List. 

                                Client the_Client = new Client();

                                the_Client.socket = (newClient);

                                the_Client.username = (incomingMessage);

                                Client_List.Add(the_Client);

                                //------------------------------------------------------------------------------------------------------------------------------------------



                                //client joined thread acception started ----------------------------------------------BREAK POINT: START THREAD ACCEPT
                                Thread receiveThread = new Thread(() => Receive(the_Client)); 
                                receiveThread.Start();

                                break;
                                //----------------------------------------------------------------------------------------------------

                            }//end of else if.

                               
                        }//foreach's

                    }//if count 0's else

               

                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        logs.AppendText("The socket stopped working.\n");
                    }

                }
            }
        }



        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------






        //------------------------------------------------------------------------------------------------------------
        //--------------------------------RECEIVE---------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------


        //----------------------------------Recieve codes-------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //----------------------------- P4CK3TR3C13V3
        //----------------------------- DUBM3SS4GER3C13V3
        //----------------------------- D3L3T10N0FF1L3
        //----------------------------- D0WNL04DF1L3
        //----------------------------- G3TAFILE
        //----------------------------- CH4NG3T0PUBL1C
        //-----------------------------
        //------------------------------------------------------------------------------------------------------------


        private void Receive(Client thisClient) // 
        {
            bool connected = true;


            while (connected && !terminating)
            {
                try
                {
                    Byte[] buffer = new Byte[1024];
                    thisClient.socket.Receive(buffer);



                    //A msg received.
                    string incoming_code = Encoding.Default.GetString(buffer);
                    incoming_code = incoming_code.Substring(0, incoming_code.IndexOf("\0"));

                    string Message = "continue";

                    //---------------------------------------------countinue cmnd-----------------------------------------
                    if (incoming_code == "C0NT1NU3")
                    {

                        //logs.AppendText("keep sending file");
                        thisClient.packet_mutex--;

                    }
                    else
                    {
                        //a feedback to continue---------------------------------------------
                        Byte[] buffer27 = Encoding.Default.GetBytes(Message);
                        try
                        {
                            thisClient.socket.Send(buffer27);
                        }
                        catch
                        {
                            logs.AppendText("There is a problem! Check the connection...\n");
                            terminating = true;
                            textBox_port.Enabled = true;
                            button_listen.Enabled = true;
                            serverSocket.Close();
                        }
                        //----------------------------------------------------------------------
                    }
                    //--------duplication part------------------------------------------------------------------



                    if (incoming_code == "DUBM3SS4GER3C13V3")
                    {


                        Byte[] buffer_message_name_of_the_file = new Byte[1024];
                        thisClient.socket.Receive(buffer_message_name_of_the_file);

                        //name of the file is received.
                        string name_of_the_dup_file = Encoding.Default.GetString(buffer_message_name_of_the_file);
                        name_of_the_dup_file = name_of_the_dup_file.Substring(0, name_of_the_dup_file.IndexOf("\0"));


                        //received_message forms as:         nickname_filename thus we can just check with it.
                        string dubs_path = folderPath + @"\" + name_of_the_dup_file + @".txt";

                        if (File.Exists(dubs_path))
                        {


                            //clones name is determined
                            string clone_file_name = name_of_the_dup_file + "(1)";
                            string clone_for_path = folderPath + @"\" + name_of_the_dup_file + @"(1).txt";

                            string counter_for_a_file_s = "";
                            int counter_for_a_file = 2;

                            //if that name exists increment the counter 
                            while (File.Exists(clone_for_path))
                            {
                                counter_for_a_file_s = counter_for_a_file.ToString();
                                clone_file_name = name_of_the_dup_file + "(" + counter_for_a_file_s + ")";

                                clone_for_path = folderPath + @"\" + name_of_the_dup_file + "(" + counter_for_a_file_s + ").txt";
                                counter_for_a_file++;
                            }

                            string whole_text = "";

                            //read
                            string[] lines = System.IO.File.ReadAllLines(dubs_path);


                            for (int i = 0; i < lines.Length; i++)
                            {
                                whole_text += lines[i];
                                whole_text += "\n";
                            }


                            //check if the file is private

                            int if_priv_is_one = 0;


                            string[] lines_db_files = System.IO.File.ReadAllLines(folderPath + @"\database_of_files.txt");
                            int that_line = 0;

                            for (int i = 0; i < lines_db_files.Length; i++)
                            {
                                if (lines_db_files[i] == name_of_the_dup_file + "=private")
                                    if_priv_is_one++;

                                if (lines_db_files[i] == name_of_the_dup_file + @"=public")
                                { }
                            }







                            //write 
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(folderPath + @"\" + clone_file_name + @".txt", true))
                            {
                                sw.WriteLine(whole_text);
                            }

                            logs.AppendText("File called " + name_of_the_dup_file + " successfully duplicated as file called: " + clone_file_name + "\n");

                            string file_name = "";

                            if(if_priv_is_one == 1)
                                file_name = clone_file_name + "=private";
                            else
                                file_name = clone_file_name + "=public";

                            //write name of the file to database
                            
                            
                            
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(folderPath + @"\database_of_files.txt", true))
                            {
                                sw.WriteLine(file_name);
                            }


                        }
                        else //if its not send an error message 
                        {
                            logs.AppendText("There is no file called " + @name_of_the_dup_file + @"!!!!" + "\n");
                            //can also send a print message
                        }






                        //write




                    }


                    //--------------------------------------duplication end-------------------------------------------



                    ///----------------------------------starting of the txt file recieve------------------------------------
                    if (incoming_code == "P4CK3TR3C13V3")
                    {

                        //The next message recieving--------------------------------------------
                        Byte[] buffer_message2 = new Byte[1024];
                        thisClient.socket.Receive(buffer_message2);

                        //The count of packets are received.
                        string incomingMessage_packet_size = Encoding.Default.GetString(buffer_message2);
                        incomingMessage_packet_size = incomingMessage_packet_size.Substring(0, incomingMessage_packet_size.IndexOf("\0"));
                        //------------------------------------------------------------------------------------------------------

                        int packcount = Int32.Parse(incomingMessage_packet_size); //packet size changed to int


                        //a feedback to continue---------------------------------------------
                        Byte[] buffer273 = Encoding.Default.GetBytes(Message);
                        try
                        {
                            thisClient.socket.Send(buffer273);
                        }
                        catch
                        {
                            logs.AppendText("There is a problem! Check the connection...\n");
                            terminating = true;
                            textBox_port.Enabled = true;
                            button_listen.Enabled = true;
                            serverSocket.Close();
                        }
                        //----------------------------------------------------------------------

                        //-------------------------------------------------file name recieve---------

                        Byte[] buffer_message242 = new Byte[1024];
                        thisClient.socket.Receive(buffer_message242);

                        //The count of packets are received.
                        string the_name_o_file = Encoding.Default.GetString(buffer_message242);
                        the_name_o_file = the_name_o_file.Substring(0, the_name_o_file.IndexOf("\0"));
                        //------------------------------------------------------------------------------------------------------


                        string fileshortname = the_name_o_file; //packet size changed to int


                        //-------------------------------------------------------------------------

                        fileName = folderPath + @"\" + fileshortname + @".txt";
                        try
                        {

                            // Check if file already exists. If yes, change the name of the file------------------- CIAE

                            //counts the replicants of files' counts.
                            string filename_counter_s = "";
                            string fileName_for_loop = fileName;

                            string fileshortname_actual = fileshortname;

                            while (File.Exists(fileName_for_loop))
                            {
                                // must add (n) if file exists.
                                filename_counter_s = filename_counter.ToString();
                                fileshortname = fileshortname_actual + "(" + filename_counter_s + ")";

                                fileName_for_loop = folderPath + @"\" + fileshortname + @".txt";
                                filename_counter++;

                            }
                            filename_counter = 1;



                            //a feedback to continue---------------------------------------------
                            Byte[] buffer271 = Encoding.Default.GetBytes(Message);
                            try
                            {
                                thisClient.socket.Send(buffer271);
                            }
                            catch
                            {
                                logs.AppendText("There is a problem! Check the connection...\n");
                                terminating = true;
                                textBox_port.Enabled = true;
                                button_listen.Enabled = true;
                                serverSocket.Close();
                            }
                            //----------------------------------------------------------------------

                            //-------------------------------------------------------name received a file created.


                            //--------------------------------------each line receiving-----------------------------

                            for (int i = 0; i < packcount; i++)
                            {

                                //The next message recieving--------------------------------------------
                                Byte[] buffer_file_msg = new Byte[1024];
                                thisClient.socket.Receive(buffer_file_msg);

                                //The count of packets are received.
                                string one_o_line = Encoding.Default.GetString(buffer_file_msg);

                                if (one_o_line[1023] == '\0')
                                     one_o_line = one_o_line.Substring(0, one_o_line.IndexOf("\0"));
                                //------------------------------------------------------------------------------------------------------

                                string one_line = one_o_line;

                                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(folderPath + @"\" + fileshortname + @".txt", true))
                                {
                                    sw.Write(@one_line);
                                }
                                // Write file contents on file.     

                                //a feedback to continue---------------------------------------------
                                Byte[] buffer2731 = Encoding.Default.GetBytes(Message);
                                try
                                {
                                    thisClient.socket.Send(buffer2731);
                                }
                                catch
                                {
                                    logs.AppendText("There is a problem! Check the connection...\n");
                                    terminating = true;
                                    textBox_port.Enabled = true;
                                    button_listen.Enabled = true;
                                    serverSocket.Close();
                                }
                                //----------------------------------------------------------------------

                            }

                            //----------------a notification printed-----------------------------

                            //server prints the a message that it receive a file from client.
                            logs.AppendText("An incoming file received called " + fileshortname + "\n");

                            string file_name = fileshortname + "=private";
                            //write name of the file to database
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(folderPath + @"\database_of_files.txt", true))
                            {
                                sw.WriteLine(file_name);
                            }
                            //----------------------------------------------------------------------



                        }
                        catch (Exception Ex)
                        {
                            Console.WriteLine(Ex.ToString());
                        }

                    }
                    ///----------------------------------end of the txt file recieve------------------------------------



                    //---------------------------------------deleltion of a file----------------------------------------

                    if (incoming_code == "D3L3T10N0FF1L3")
                    {




                        Byte[] buffer_message_name_of_the_file = new Byte[1024];
                        thisClient.socket.Receive(buffer_message_name_of_the_file);

                        //name of the file is received.
                        string name_of_the_del_file = Encoding.Default.GetString(buffer_message_name_of_the_file);
                        name_of_the_del_file = name_of_the_del_file.Substring(0, name_of_the_del_file.IndexOf("\0"));


                        //received_message forms as:         nickname_filename thus we can just check with it.
                        string dubs_path = folderPath + @"\" + name_of_the_del_file + @".txt";

                        if (File.Exists(dubs_path))
                        {

                            FileInfo f_deleted = new FileInfo(dubs_path);

                            //file deleted
                            f_deleted.Delete();


                            //must be deleted from the database of files.

                            string[] lines_db_files = System.IO.File.ReadAllLines(folderPath + @"\database_of_files.txt");
                            int that_line = 0;

                            for (int i = 0; i < lines_db_files.Length; i++)
                            {
                                if (lines_db_files[i] == name_of_the_del_file + "=private" || lines_db_files[i] == name_of_the_del_file + "=public")
                                {
                                    that_line = i;
                                    break;
                                }
                            }

                            //recreated the database of files.

                            //database of the name of the file lists.
                            if (File.Exists(folderPath + @"\database_of_files.txt"))
                            {
                                File.Delete(folderPath + @"\database_of_files.txt");
                            }

                            for (int i = 0; i < lines_db_files.Length; i++)
                            {
                                if (i != that_line)
                                {
                                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(folderPath + @"\database_of_files.txt", true))
                                    {
                                        sw.WriteLine(lines_db_files[i]);
                                    }

                                }
                            }

                            logs.AppendText("File called " + name_of_the_del_file + @" is deleted from server!!!!" + "\n");


                        }
                        else //if its not send an error message 
                        {
                            logs.AppendText("A deletion requested but there is no file called " + name_of_the_del_file + @"!!!!" + "\n");
                            //can also send a print message
                        }





                    }
                    //----------------------------------------------------------------------------------------------------



                    //-----------------------------------------download file requested-----------------------------------
                    if (incoming_code == "D0WNL04DF1L3")
                    {

                        Byte[] buffer_message_name_of_the_file = new Byte[1024];
                        thisClient.socket.Receive(buffer_message_name_of_the_file);

                        //name of the file is received.
                        string name_of_the_sent_file = Encoding.Default.GetString(buffer_message_name_of_the_file);
                        name_of_the_sent_file = name_of_the_sent_file.Substring(0, name_of_the_sent_file.IndexOf("\0"));


                        //received_message forms as:         nickname_filename thus we can just check with it.
                        string dubs_path = folderPath + @"\" + name_of_the_sent_file + @".txt";


                        //reads database and find where the line where the file exists
                        string[] lines_db_files = System.IO.File.ReadAllLines(folderPath + @"\database_of_files.txt");
                        int that_line = 0;

                        int private_alert = 0;

                        private_alert = 0;

                        for (int i = 0; i < lines_db_files.Length; i++)
                        {
                            if (lines_db_files[i] == name_of_the_sent_file + "=private")
                            {
                                private_alert++;
                            }
                        }


                        for (int i = 0; i < name_of_the_sent_file.Length; i++)
                        {

                            if (name_of_the_sent_file[i] == '_')
                            {
                               
                                if(name_of_the_sent_file.Substring(0, i) == thisClient.username)
                                {private_alert--;}
                            }
                               
                            
                        }





                        //if received file is private then print error and do not send 


                        if (private_alert == 1)
                        {
                            logs.AppendText("The file that is requested is private!\n");
                        }



                        else
                        {

                            if (File.Exists(dubs_path))
                            {

                                //check if the user send this or the file is public?

                                //---------------------------FILE SENT ALGORITHM's MUTEX IS ACTIVATED
                                //------------------------------------------------
                                //send that file is exist

                                string valid_for_download = "Download_is_valid";

                                byte[] div = ASCIIEncoding.ASCII.GetBytes(valid_for_download);
                                thisClient.socket.Send(div);



                                Thread sent_file = new Thread(() => send_file(thisClient, dubs_path, name_of_the_sent_file));
                                sent_file.Start();


                                //--------------------------------------------------------------------
                                //--------------------------------------------------------------------
                            }
                            else //if its not send an error message 
                            {
                                logs.AppendText("A file is requested but there is no file called " + name_of_the_sent_file + @"!!!!" + "\n");


                                //can also send a print message
                            }
                        }

                    }

                    //-----------------------------------------Send the file list to client--------------------------------------------------------------
               

                    if (incoming_code == "G3TAFILE")
                    {

                        Byte[] buffer_message_name_of_the_username = new Byte[1024];
                        thisClient.socket.Receive(buffer_message_name_of_the_username);

                        int empty_counter = 0;

                        //name of the user is received.
                        string username_received = Encoding.Default.GetString(buffer_message_name_of_the_username);
                        username_received = username_received.Substring(0, username_received.IndexOf("\0"));

                        string dubs_path = folderPath + @"\" + "database_of_files" + @".txt";
                        string[] lines = System.IO.File.ReadAllLines(dubs_path);
                        int number_at_list = 0;

                        // this int prevents the double send of owned public files.
                        int double_preventer = 0;


                        //Check each line in database of files for either it is public or owned if that so send these file name and file datas.
                        foreach (string line in lines)
                        {
                       
                            double_preventer = 0;


                            for (int i = 0; i < line.Length; i++)
                            {

                                if (line[i] == '_')
                                {
                                    if (line.Substring(0, i) == username_received)
                                    {
                                        for (int k = i; k < line.Length; k++)
                                        {
                                            if (line[k] == '=')
                                            {
                                                
                                                string files_own_name = line.Substring(i + 1, k - i - 1);
                                                string pathforsize = folderPath + @"\" + username_received +"_" + files_own_name + @".txt";

                                                DateTime timeofcreation = File.GetCreationTime(pathforsize);


                                                long the_size_off = new System.IO.FileInfo(pathforsize).Length;
                                                string size_offile = the_size_off.ToString();

                                                string the_time = timeofcreation.ToString();


                                                string send_msg = files_own_name + " " + size_offile + " bytes" + " at: " + the_time;

                                                empty_counter++;

                                                //send
                                                Byte[] buffer34 = new Byte[1024];
                                                buffer34 = Encoding.Default.GetBytes("G3TAFILE");
                                                thisClient.socket.Send(buffer34);

                                                System.Threading.Thread.Sleep(400);

                                                Byte[] buffer35 = new Byte[1024];
                                                buffer35 = Encoding.Default.GetBytes(send_msg);

                                                double_preventer++;

                                                thisClient.socket.Send(buffer35);
                                                logs.AppendText("file-list item for specific user is sent\n");
                                                number_at_list += 1;
                                                break;

                                            }
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            System.Threading.Thread.Sleep(400);

                            for (int i = 0; i < line.Length; i++)
                            {

                                if (line[i] == '=' && double_preventer == 0)
                                {
                                    if (line.Substring(i+1,6) == "public" )
                                    {

                                                string files_own_name = line.Substring(0, i);
                                                string pathforsize = folderPath + @"\" + files_own_name + @".txt";

                                                DateTime timeofcreation = File.GetCreationTime(pathforsize);


                                                long the_size_off = new System.IO.FileInfo(pathforsize).Length;
                                                string size_offile = the_size_off.ToString();

                                                string the_time = timeofcreation.ToString();

                                                string send_msg = files_own_name + " " + size_offile + " bytes" + " at: " + the_time;

                                                empty_counter++;

                                               //send
                                                Byte[] buffer34 = new Byte[1024];
                                                buffer34 = Encoding.Default.GetBytes("G3TAFILE");
                                                thisClient.socket.Send(buffer34);

                                                System.Threading.Thread.Sleep(400);

                                                Byte[] buffer352 = new Byte[1024];
                                                buffer352 = Encoding.Default.GetBytes(send_msg);


                                                thisClient.socket.Send(buffer352);
                                                logs.AppendText("file-list item for specific user is sent\n");
                                                number_at_list += 1;
                                                break;

                                         
                                    }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }


                            System.Threading.Thread.Sleep(400);

                        }                  
                        
                        // if no file name and data is sent to client than send this message which says no file for client is available for download.
                        if (empty_counter == 0)
                            {
                                //send
                                Byte[] buffer34 = new Byte[1024];
                                buffer34 = Encoding.Default.GetBytes("G3TAFILE");
                                thisClient.socket.Send(buffer34);

                                System.Threading.Thread.Sleep(400);

                                Byte[] buffer352 = new Byte[1024];
                                buffer352 = Encoding.Default.GetBytes("No file found");
                                thisClient.socket.Send(buffer352);

                            }
                    }

                    //---------------------------------------------------------------------------------------------------


                    //----------------------------------CHANGE TO PUBLIC---------------------------------------------------


                    if (incoming_code == "CH4NG3T0PUBL1C")
                    {

                        Byte[] buffer_message_name_of_the_file = new Byte[1024];
                        thisClient.socket.Receive(buffer_message_name_of_the_file);

                        //name of the file is received.
                        string name_of_the_changed_file = Encoding.Default.GetString(buffer_message_name_of_the_file);
                        name_of_the_changed_file = name_of_the_changed_file.Substring(0, name_of_the_changed_file.IndexOf("\0"));


                        //received_message forms as:         nickname_filename thus we can just check with it.
                        string dubs_path = folderPath + @"\" + name_of_the_changed_file + @".txt";

                        if (File.Exists(dubs_path))
                        {

                            //must be refreshed from the database of files.

                            string[] lines_db_files = System.IO.File.ReadAllLines(folderPath + @"\database_of_files.txt");
                            int that_line = 0;

                            for (int i = 0; i < lines_db_files.Length; i++)
                            {
                                if (lines_db_files[i] == name_of_the_changed_file + "=private" || lines_db_files[i] == name_of_the_changed_file + "=public")
                                {
                                    that_line = i;
                                    break;
                                }
                            }

                            //recreated the database of files.

                            //database of the name of the file lists.
                            if (File.Exists(folderPath + @"\database_of_files.txt"))
                            {
                                File.Delete(folderPath + @"\database_of_files.txt");
                            }

                            for (int i = 0; i < lines_db_files.Length; i++)
                            {
                                if (i != that_line)
                                {
                                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(folderPath + @"\database_of_files.txt", true))
                                    {
                                        sw.WriteLine(lines_db_files[i]);
                                    }

                                }
                                if (i == that_line)
                                {
                                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(folderPath + @"\database_of_files.txt", true))
                                    {
                                        sw.WriteLine(name_of_the_changed_file + "=public");
                                    }
                                }
                            }

                            logs.AppendText("File called " + name_of_the_changed_file + @" is changed to public." + "\n");
               


                        }
                        else //if its not send an error message 
                        {
                            logs.AppendText("There is no file called " + name_of_the_changed_file + @"!!!!" + "\n");
                            //can also send a print message
                        }






                    }






                }


                catch
                {
                    if (!terminating)
                    {
                        logs.AppendText("The client called " + thisClient.username + " disconnected\n");
                    }

               


                    thisClient.socket.Close();
                    Client_List.Remove(thisClient);
                    connected = false;
                }
            }
        }


        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------


        //------------------------------------------------------------------------------------------------------------
        //---------------------------------------------SEND_FILE-----------------------------------------------------
        //------------------------------------------------------------------------------------------------------------



        private void send_file(Client thisClient, string path, string shorfilename) // 
        {

            thisClient.packet_mutex = 0;

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
               string real_short_file_name = shorfilename;

            //-------------------------------------------------------RNFK

            //--------------firstly must send the code for file sending--------------------

            string send_file_code = "P4CK3TR3C13V3";

            byte[] LogoDataBy = ASCIIEncoding.ASCII.GetBytes(send_file_code);
            thisClient.socket.Send(LogoDataBy);

            thisClient.packet_mutex++;

            //------------------------------------------------------------------------------



            while (thisClient.packet_mutex != 0)
            { }

            //----------------------------------------------find the count of packs and send it-------------

            // --------------- a little demo

            int demo_counter = 0;
            char demo_string;


            List<char> demo_char = new List<char>();

            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    //Console.Write((char)sr.Read());
                    int demo_i = sr.Read();

                    if (demo_i != 13)
                    {
                        //demo_string = (char)demo_i;

                        //demo_char.Add(demo_string);

                        //logs.AppendText(demo_string + "");
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

            thisClient.socket.Send(LogoDataBy_pack);
            thisClient.packet_mutex++;

            //--------------------------------------------------



            //SLEEP COMMAND
            while (thisClient.packet_mutex != 0)
            { }


            //--------------------------------------------file name sending--------------------------------------

            string filenamesent = real_short_file_name;

            byte[] files_name = ASCIIEncoding.ASCII.GetBytes(filenamesent);

            thisClient.socket.Send(files_name);
            thisClient.packet_mutex++;



            //SLEEP COMMAND and mutex
            while (thisClient.packet_mutex != 0)
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

                            thisClient.socket.Send(msg_byte);
                            thisClient.packet_mutex++;


                            while (thisClient.packet_mutex != 0)
                            { }

                            the_message = "";


                        }


                    }
                }
            }

            if (pack_count % 1024 != 0)
            {
                byte[] msg_byte = ASCIIEncoding.ASCII.GetBytes(the_message);

                thisClient.socket.Send(msg_byte);
                thisClient.packet_mutex++;


                while (thisClient.packet_mutex != 0)
                { }
            }










        }



        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------




        //------------------------------------------------------------------------------------------------------------
        //---------------------------------------------FROM 1 CLOSING-------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }


        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------












        //------------------------------------------------------------------------------------------------------------
        //--------------------------------------BUTTON SEND-----------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------

        //private void button_send_Click(object sender, EventArgs e)
        //{
        //    string message = textBox_message.Text;
        //    if(message != "" && message.Length <= 64)
        //    {
        //        Byte[] buffer = Encoding.Default.GetBytes(message);
        //        foreach (Socket client in clientSockets)
        //        {
        //            try
        //            {
        //                client.Send(buffer);
        //            }
        //            catch
        //            {
        //                logs.AppendText("There is a problem! Check the connection...\n");
        //                terminating = true;
        //                textBox_message.Enabled = false;
        //                button_send.Enabled = false;
        //                textBox_port.Enabled = true;
        //                button_listen.Enabled = true;
        //                serverSocket.Close();
        //            }

        //        }
        //    }
        //}


        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------







        //-----------------------------------------------------------------------------------------------------------
        //-----------BROWSE BUTTON-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------



        private void Browse_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog folderBrowser = new OpenFileDialog();
   
            //select a file in order to use for keeping our receive files and database.txt file.

            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;
            
            folderBrowser.FileName = "Folder Selection.";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                folderPath = Path.GetDirectoryName(folderBrowser.FileName);
                logs.AppendText("Path: " + folderPath + "\n");
                
                //path is coppied to a global int in order to used in other funcitons
                the_path = folderPath;

                button_listen.Enabled = true;
                textBox_port.Enabled = true;

                //Created a file called database to keep the all usernames of the users logged in during the session.
                fileName = folderPath + @"\database.txt";
                string dbfileName = folderPath + @"\database_of_files.txt";
                try
                {
                    // Check if the file exists or not. If so, delete it....   
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    using (System.IO.StreamWriter sw =
             new System.IO.StreamWriter(folderPath + @"\database.txt", true))
                    {
                        sw.WriteLine(incomingMessage);
                    }

                    //database of the name of the file lists.
                    if (File.Exists(dbfileName))
                    {
                        File.Delete(dbfileName);
                    }

                    using (System.IO.StreamWriter sw =
                    new System.IO.StreamWriter(folderPath + @"\database_of_files.txt", true))
                    {
                        sw.WriteLine(incomingMessage);
                    }





                    button_listen.Enabled = true;
                    textBox_port.Enabled = true;
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.ToString());
                }
            }

           
        }



        //--------------------------------------------------
        //--------------------------------------------------
        //--------------------------------------------------







    }
}
