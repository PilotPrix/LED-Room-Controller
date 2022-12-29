﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace LED_Room_Controller
{
    public partial class Form1 : Form
    {
        static SerialPort arduino;
        static string port = "COM5";
        ColorDialog colorPicker = new ColorDialog();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            arduino = new SerialPort(port, 9600);
            arduino.ReadTimeout = 10;
            openArduino();
        }

        private void openArduino()
        {
            bool success = false;

            while (!success)
            {
                try
                {
                    arduino.Open();
                    success = true;
                }
                catch
                {
                    DialogResult result = MessageBox.Show("Unable to connect to serial port: " + port, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    Console.WriteLine(result);
                    if (result == DialogResult.Cancel)
                    {
                        Application.Exit();
                        return;
                    }
                }
            }
        }


        private void allOnButton_Click(object sender, EventArgs e)
        {
            arduino.WriteLine("255150");
            toggleStrip.Checked = true;
            toggleNeopixelStrip.Checked = true;
        }

        private void allOffButton_Click(object sender, EventArgs e)
        {
            arduino.WriteLine("000000");
            toggleStrip.Checked = false;
            toggleNeopixelStrip.Checked = false;
        }

        private void colorPickerButton_Click(object sender, EventArgs e)
        {
            colorPicker.ShowDialog();
            string r = constantifyInt(colorPicker.Color.R);
            string g = constantifyInt(colorPicker.Color.G);
            string b = constantifyInt(colorPicker.Color.B);

            arduino.WriteLine("Color " + r + " " + g + " " + b);
        }

        private string constantifyInt(int num)
        {
            if (num >= 100)
            {
                return num.ToString();
            }
            else if (num >= 10)
            {
                return " " + num.ToString();
            }
            else
            {
                return "  " + num.ToString();
            }
        }

        private void toggleStrip_CheckedChanged(object sender, EventArgs e)
        {
            if (toggleStrip.Checked)
            {
                arduino.WriteLine("255   ");
            }
            else
            {
                arduino.WriteLine("0     ");
            }
        }

        private void toggleNeopixelStrip_CheckedChanged(object sender, EventArgs e)
        {
            if (toggleNeopixelStrip.Checked)
            {
                arduino.WriteLine("   150");
            }
            else
            {
                arduino.WriteLine("   000");
            }
        }
    }
}