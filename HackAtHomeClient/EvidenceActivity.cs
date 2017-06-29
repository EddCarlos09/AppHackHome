using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HackAtHome.CustomAdapters;
using HackAtHome.SAL;
using HackAtHome.Entities;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = false, Icon = "@drawable/Icon")]
    public class EvidenceActivity : Activity
    {
        EvidencesFragment Data;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Evidence);
            //Get String Extra
            string Token = Intent.GetStringExtra("Token");
            string UserName = Intent.GetStringExtra("UserName");
            //Set Txt User Name value UserName
            var TextUserName = FindViewById<TextView>(Resource.Id.TextViewUserName);
            TextUserName.Text = UserName;

            Data = (EvidencesFragment)this.FragmentManager.FindFragmentByTag("Data");
            if (Data == null)
            {
                Data = new EvidencesFragment();
                ServiceClient ServiceClient = new ServiceClient();
                List<Evidence> EvidencesList = await ServiceClient.GetEvidencesAsync(Token);
                var FragmentTransaction = this.FragmentManager.BeginTransaction();
                Data.EvidencesList = EvidencesList;
                FragmentTransaction.Add(Data, "Data");
                FragmentTransaction.Commit();
            }
            var ListEvidences = FindViewById<ListView>(Resource.Id.ListViewEvidences);
            ListEvidences.Adapter = new EvidencesAdapter(this, Data.EvidencesList, Resource.Layout.EvidenceItem, Resource.Id.TextViewTitleEvidence, Resource.Id.TextViewStatusEvidence);

            ListEvidences.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                EvidencesAdapter AdapterEv = (EvidencesAdapter)ListEvidences.Adapter;
                Evidence SelectedItem = AdapterEv[e.Position];
                var EvidenceDetailIntent = new Android.Content.Intent(this, typeof(EvidenceDetailActivity));
                EvidenceDetailIntent.PutExtra("EvidenceID", SelectedItem.EvidenceID);
                EvidenceDetailIntent.PutExtra("EvidenceTitle", SelectedItem.Title);
                EvidenceDetailIntent.PutExtra("EvidenceStatus", SelectedItem.Status);
                EvidenceDetailIntent.PutExtra("UserName", UserName);
                EvidenceDetailIntent.PutExtra("Token", Token);
                StartActivity(EvidenceDetailIntent);
            };
        }
    }
}