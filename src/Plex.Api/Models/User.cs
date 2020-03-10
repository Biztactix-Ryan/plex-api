﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Plex.Api.Helpers;

namespace Plex.Api.Models
{
    public class PlexAccount
    {
        [JsonPropertyName("user")]
        public User User { get; set; }
    }

    /// <summary>
    /// Attributes:
    /// SIGNIN (str): 'https://plex.tv/users/sign_in.xml'
    ///     key (str): 'https://plex.tv/users/account.json'
    ///     authenticationToken (str): Unknown.
    ///     certificateVersion (str): Unknown.
    ///     cloudSyncDevice (str): Unknown.
    ///     email (str): Your current Plex email address.
    ///     entitlements (List&lt;str&gt;): List of devices your allowed to use with this account.
    ///     guest (bool): Unknown.
    ///     home (bool): Unknown.
    ///     homeSize (int): Unknown.
    ///     id (str): Your Plex account ID.
    ///     locale (str): Your Plex locale
    ///     mailing_list_status (str): Your current mailing list status.
    ///     maxHomeSize (int): Unknown.
    ///     queueEmail (str): Email address to add items to your `Watch Later` queue.
    ///     queueUid (str): Unknown.
    ///     restricted (bool): Unknown.
    ///     roles: (List&lt;str&gt;) Lit of account roles. Plexpass membership listed here.
    ///     scrobbleTypes (str): Description
    ///     secure (bool): Description
    ///     subscriptionActive (bool): True if your subsctiption is active.
    ///     subscriptionFeatures: (List&lt;str&gt;) List of features allowed on your subscription.
    ///     subscriptionPlan (str): Name of subscription plan.
    ///     subscriptionStatus (str): String representation of `subscriptionActive`.
    ///     thumb (str): URL of your account thumbnail.
    ///     title (str): Unknown. - Looks like an alias for `username`.
    ///     username (str): Your account username.
    ///     uuid (str): Unknown.
    /// </summary>
    public class User
    {
        [JsonPropertyName("id")]
        [JsonConverter(typeof(IntValueConverter))]
        public int Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }
        
        [JsonPropertyName("joined_at")]
        public DateTime JoinedAt { get; set; }
        
        [JsonPropertyName("username")]
        public string Username { get; set; }
        
        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        [JsonPropertyName("thumb")]
        public string Thumb { get; set; }
        
        [JsonPropertyName("hasPassword")]
        public bool HasPassword { get; set; }

        [JsonPropertyName("authentication_token")]
        public string AuthenticationToken { get; set; }
        
        [JsonPropertyName("confirmedAt")]
        public DateTime ConfirmedAt { get; set; }
        
        [JsonPropertyName("forumId")]
        public int? ForumId { get; set; }
        
        [JsonPropertyName("rememberMe")]
        public bool RememberMe { get; set; }
        
        [JsonPropertyName("subscription")]
        public Subscription Subscription { get; set; }
        
        [JsonPropertyName("roles")]
        public Roles Roles { get; set; }
        
        [JsonPropertyName("entitlements")]
        public List<string> Entitlements { get; set; }
    }
}