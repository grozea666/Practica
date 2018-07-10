using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public void Update()
        {
            while (true)
            {
                StartClient("");
                Thread.Sleep(500);
            }
        }
        public void StartClient(string text)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                //string text = Console.ReadLine();
                text += "<EOF>";
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.  
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(remoteEP);

                    //Console.WriteLine("Socket connected to {0}",
                    // sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.  
                    byte[] msg = Encoding.ASCII.GetBytes(text);

                    // Send the data through the socket.  
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.  
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("{0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                    //return Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    //textBox2.Text = Encoding.ASCII.GetString(bytes, 0, bytesRec) + Environment.NewLine;
                    textBox2.Invoke((Action)delegate

                    {

                        textBox2.Text = Encoding.ASCII.GetString(bytes, 0, bytesRec) + Environment.NewLine;

                    });
                    //textBox1.Text = "";
                    //StartClient();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            
        }
        public Form1()
        {
            InitializeComponent();
            Thread thread = new Thread(new ThreadStart(Update));
            thread.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Display logs from database
            textBox3.Text = "";
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Sokar\Documents\Audit.mdf; Integrated Security=True;");
            string query = "Select * From [Table]";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dataRow in dt.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    textBox3.Text += item + Environment.NewLine;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Generate key
            textBox3.Text = "";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //chat
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //enter chat text
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //send chat button
            //Activity activity = new Activity();
            //activity.SQLDatabase(textBox1.Text);
            //textBox2.Text +=/*username from database +*/"local:" + textBox1.Text+Environment.NewLine;
            //textBox1.Text = "";
            //Thread thread = new Thread(new ThreadStart(Update));
            //thread.Start();
            StartClient("local:" + textBox1.Text + Environment.NewLine);
            textBox1.Text = "";
            //textBox2.Text = StartClient("local:" + textBox1.Text + Environment.NewLine);
            //textBox1.Text = "";
            //Update();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //revocare key
            textBox3.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //gestionare utilizatori
            textBox3.Text = "";
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Sokar\Documents\data.mdf;Integrated Security=True;Connect Timeout=30;");
            string query = "Select Username From [Table]";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dataRow in dt.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    textBox3.Text += item + Environment.NewLine;
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //text for options
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //input for options
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //send input for options
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Sokar\Documents\data.mdf;Integrated Security=True;Connect Timeout=30;");
            string query = "Update [Table] SET active='false' Where Username=" + textBox4.Text + "";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //send input for options
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Sokar\Documents\data.mdf;Integrated Security=True;Connect Timeout=30;");
            string query = "Update [Table] SET active='true' Where Username=" + textBox4.Text + "";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
        }
    }
    public class Activity
    {
        public void SQLDatabase(string bcd)
        {

            //String user = WindowsFormsApp1.Login.username;
            string user = "local";

            String query = "INSERT INTO [Table] VALUES ('" + user + "','" + bcd + "','" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "')";
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Sokar\Documents\Audit.mdf; Integrated Security=True;");
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Database Error");
            }
        }
    }
}
