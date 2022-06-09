﻿using Device;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManager.Device.DeviceMethods
{
    public class StopMethod
    {

        public static async Task<MethodResponse> OnStop(MethodRequest methodRequest, object userContext)
        {
            if (Program.UserData != null)
            {
                Program.IsRunning = false;
                Program.DeviceIsInUse = false;

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                Console.WriteLine("Lägnden på listan: " + Program.UserData.TrainingData.Count + " rader");
                Console.WriteLine("Device stopped");
                Console.WriteLine("Username: " + Program.UserData.ObjectId);

                string trainingData = JsonConvert.SerializeObject(Program.UserData);
                await SendTrainingDataToIoTHubAsync(trainingData);
                Console.WriteLine(stopwatch.ElapsedMilliseconds + " millisekunder tog det att ladda upp datan");
                Program.UserData.TrainingData.Clear();
            }

            return null;
        }


        public static async Task SendTrainingDataToIoTHubAsync(string dataToSend)
        {
            Encoding messageEncoding = Encoding.UTF8;

            using Message msg = new(messageEncoding.GetBytes(dataToSend));

            await Program.Client.SendEventAsync(msg);
        }

    }
}
