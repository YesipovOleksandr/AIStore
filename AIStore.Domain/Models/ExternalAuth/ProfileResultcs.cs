using Newtonsoft.Json;

namespace AIStore.Domain.Models.ExternalAuth
{
    public class ProfileResult
    {
        public ProfileResult(string id, string email, string firstName, string lastName, string photoUrl)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            PhotoUrl = photoUrl;
        }

        public string Id { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhotoUrl { get; set; }
    }

    public class BaseProfileResult
    {
        public string Id { get; set; }
    }

    public class GoogleProfileResult : BaseProfileResult
    {
        public string Email { get; set; }

        [JsonProperty("given_name")]
        public string FirstName { get; set; }

        [JsonProperty("family_name")]
        public string LastName { get; set; }

        [JsonProperty("picture")]
        public string PhotoUrl { get; set; }
    }

    public class FacebookProfileResult : BaseProfileResult
    {
        public string Email { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }

    public class LinkedinProfileResult : BaseProfileResult
    {
        public class LastNameItem
        {
            [JsonProperty("localized")]
            public localized localized { get; set; }
        }

        public class FirstNameItem
        {
            [JsonProperty("localized")]
            public localized localized { get; set; }
        }

        public class localized
        {
            [JsonProperty("ru_RU")]
            public string lang { get; set; }
        }

        public class ElementsItem
        {
            [JsonProperty("handle~")]
            public handle handle { get; set; }
        }

        public class handle
        {
            [JsonProperty("emailAddress")]
            public string emailAddress { get; set; }
        }

        [JsonProperty("elements")]
        public List<ElementsItem> Elements { get; set; }

        [JsonProperty("firstName")]
        public FirstNameItem FirstName { get; set; }

        [JsonProperty("lastName")]
        public LastNameItem LastName { get; set; }
    }

}
