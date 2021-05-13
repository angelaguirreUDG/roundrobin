using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RoundRobin
{
    public partial class Form1 : Form
    {
        List<Process> loader;
        Queue<Process> processes;

        public Form1()
        {
            InitializeComponent();

            processesChart.Titles.Add("Planificacion de procesos");

            loader    = new List<Process>();
            processes = new Queue<Process>();

        }


        //Craga los procesos a una lista para despues agregarlos a la cola
        private void LoadProcess()
        {
            loader.Add(new Process("Tarea 1", 60, 3000));
            loader.Add(new Process("Tarea 2", 50, 3500));
            loader.Add(new Process("Tarea 3", 100, 5000));
            loader.Add(new Process("Tarea 4", 90, 4500));
            loader.Add(new Process("Tarea 5", 100, 3000));
            loader.Add(new Process("Tarea 6", 80, 2500));
            loader.Add(new Process("Tarea 7", 80, 2000));
            loader.Add(new Process("Tarea 8", 60, 4000));
            loader.Add(new Process("Tarea 9", 50, 3000));
            loader.Add(new Process("Tarea 10", 40, 800));

            
        }

        //Agrega los procesos a la cola
        private void AddToQueue()
        {
            foreach (Process process in loader)
            {

                Series serie = processesChart.Series.Add(process.Name);
                serie.Label = process.Burst.ToString();
                serie.Points.Add(process.Burst);
                serie.Color = Color.LightSkyBlue;
                serie.BorderColor = Color.Black;
                processesChart.Series[process.Name]["PointWidth"] = "2";

                processes.Enqueue(process);

                WaitMiliseconds(100);
            }
        }

        //Hace una espera de n milisegundos
        private void WaitMiliseconds(int miliseconds)
        {
            if (miliseconds < 1) return;
            DateTime _desired = DateTime.Now.AddMilliseconds(miliseconds);
            while (DateTime.Now < _desired)
            {
                System.Windows.Forms.Application.DoEvents();
            }

        }


        //Cambia el estado de las series para que se vaya reduciendo su tamaño
        private void ChangeSeries(Process process, string action)
        {
            if(action == "burst")
                WaitMiliseconds(process.Burst);
            else
                WaitMiliseconds(process.Quantum);

            processesChart.Series[process.Name].Label = process.Burst.ToString();
            processesChart.Series[process.Name].Points.Clear();
            processesChart.Series[process.Name].Points.Add(process.Burst);
        }


        //Algoritmo de Round Robin
        private void RoundRobin()
        {
            Process aux;

            while (processes.Count != 0)
            {
                aux = processes.Dequeue();

                //Si la rafaga de CPU es menor que el quantum
                if (aux.Burst <= aux.Quantum) {

                    aux.Burst  = 0;
                    aux.Status = "Eejecucion";
                    processesChart.Series[aux.Name].Color = Color.Green;

                    ChangeSeries(aux, "burst");

                    processesChart.Series[aux.Name].Color = Color.Black;
                    aux.Status = "Terminado";
                    processesChart.Series.Remove(processesChart.Series[aux.Name]);
                    
                } else {
                    aux.Burst -= aux.Quantum;
                    processesChart.Series[aux.Name].Color = Color.Green;

                    ChangeSeries(aux, "quantum");

                    aux.Status = "Bloqueado";
                    processesChart.Series[aux.Name].Color = Color.Red;

                    WaitMiliseconds(100);

                    processesChart.Series[aux.Name].Color = Color.SkyBlue;
                    processes.Enqueue(aux);
                }

            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            
            loader.Clear();
            processes.Clear();

            LoadProcess();
            AddToQueue();
            RoundRobin();
        }
    }
}
