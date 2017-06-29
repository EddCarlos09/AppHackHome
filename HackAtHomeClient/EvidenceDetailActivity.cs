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
using HackAtHome.Entities;
using HackAtHome.SAL;
using Android.Graphics;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = false, Icon = "@drawable/Icon")]
    public class EvidenceDetailActivity : Activity
    {
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EvidenceDetail);

            int EvidenceID = Intent.GetIntExtra("EvidenceID", 0);
            string EvidenceTitle = Intent.GetStringExtra("EvidenceTitle");
            string EvidenceStatus = Intent.GetStringExtra("EvidenceStatus");
            string UserName = Intent.GetStringExtra("UserName");
            string Token = Intent.GetStringExtra("Token");
            var TextUserName = FindViewById<TextView>(Resource.Id.TextViewUserNameEvidence);
            var TextEvidenceTitle = FindViewById<TextView>(Resource.Id.TextViewEvidenceTitle);
            var TextEvidenceStatus = FindViewById<TextView>(Resource.Id.TextViewEvidenceStatus);
            var Image = FindViewById<ImageView>(Resource.Id.ImageView);
            var WebView = FindViewById<Android.Webkit.WebView>(Resource.Id.WebViewDesc);
            WebView.SetBackgroundColor(Color.WhiteSmoke);
            TextUserName.Text = UserName;
            TextEvidenceTitle.Text = EvidenceTitle;
            TextEvidenceStatus.Text = EvidenceStatus;

            ServiceClient ServiceClient = new ServiceClient();
            EvidenceDetail ItemEvidenceDetail = await ServiceClient.GetEvidenceByIDAsync(Token, EvidenceID);
            
            Koush.UrlImageViewHelper.SetUrlDrawable(Image, ItemEvidenceDetail.Url);
            WebView.LoadDataWithBaseURL(null,ItemEvidenceDetail.Description, "text/html", "utf-8", null);

        }
        
    }
}