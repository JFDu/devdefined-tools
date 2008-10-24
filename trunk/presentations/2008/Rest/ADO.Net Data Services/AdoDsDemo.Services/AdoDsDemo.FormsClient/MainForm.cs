using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Services.Client;
using AdoDsDemo.FormsClient.Models;
using System.IO;
using AdoDsDemo.FormsClient.Utility;
using System.Net;

namespace AdoDsDemo.FormsClient
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private DataServiceContext CreateContext()
        {
            var context = new DataServiceContext(new Uri(this.serverUrl.Text));
            context.SendingRequest += new EventHandler<SendingRequestEventArgs>(context_SendingRequest);            
            return context;
        }

        void context_SendingRequest(object sender, SendingRequestEventArgs e)
        {
            // NOTE: the http method is always GET - the SendingRequest event is fired
            // before ADO.Net data services sets the apropriate VERB.

            this.textBox1.AppendText(e.Request.FormatRequest());
        }

        private void linqQueryButton_Click_1(object sender, EventArgs e)
        {
            var context = CreateContext();
            var currencies = context.CreateQuery<Currency>("Currency").OrderBy(d => d.Name);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = currencies.ToList();            
        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            var context = CreateContext();
            context.MergeOption = MergeOption.AppendOnly;

            Currency currency = new Currency
            {
                CurrencyCode = "FBC",
                Name = "Fallout Bottle Cap"
            };

            context.AddObject("Currency", currency);

            var response = context.SaveChanges();

            this.textBox1.AppendText(response.FormatResponse());
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            var context = CreateContext();

            Currency fbcCurrency = null;

            try
            {
                fbcCurrency = context.CreateQuery<Currency>("Currency")
                    .Where(currency => currency.CurrencyCode == "FBC").ToList().FirstOrDefault();
            }
            catch (WebException ex)
            {
                if (((HttpWebResponse)ex.Response).StatusCode != HttpStatusCode.NotFound) throw;
            }

            if (fbcCurrency != null)
                context.DeleteObject(fbcCurrency);

            this.textBox1.AppendText(context.SaveChanges().FormatResponse());
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            dataGridView1.DataSource = null;
        }        

        private void batchInsertButton_Click(object sender, EventArgs e)
        {
            // build a list of currencies

            var currenciesList = new List<Currency>
            {
                new Currency { CurrencyCode = "AA1", Name = "Test Currency 1"},
                new Currency { CurrencyCode = "AA2", Name = "Test Currency 2"},
                new Currency { CurrencyCode = "AA3", Name = "Test Currency 3"},
            };
            
            // create a context

            var context = CreateContext();

            // add the currencies to the context (but like a unit of work / session)

            foreach (var currencyToAdd in currenciesList) 
                context.AddObject("Currency", currencyToAdd);

            try
            {
                this.textBox1.AppendText("\r\n\r\nAdding 3 new currencies in batch ...");

                // flush out the changes, using SaveChanges - we pass an additional option
                // to use a single batch.

                var response = context.SaveChanges(SaveChangesOptions.Batch);

                // batching will send all changes in a single request (using mime-encoding)... tunelling
                // requests like this is a little frowned upon... but it can be very useful.

                this.textBox1.AppendText(response.FormatResponse());
            }
            catch (Exception ex)
            {
                this.textBox1.Text = ex.ToString();
                return;
            }

            // cleanup (delete the currencies we added)

            this.textBox1.AppendText("\r\n\r\nCleaning up (deleting currencies) ...");

            foreach (var currencyToDelete in currenciesList)
                context.DeleteObject(currencyToDelete);

            context.SaveChanges();            
        }
    }
}
