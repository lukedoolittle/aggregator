using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class FitbitFeatures
    {
        [DataMember(Name = "exerciseGoal")]
        [JsonProperty(PropertyName = "exerciseGoal")]
        public bool ExerciseGoal { get; set; }
    }

    [DataContract]
    public class FitbitTopBadge
    {
        [DataMember(Name = "badgeGradientEndColor")]
        [JsonProperty(PropertyName = "badgeGradientEndColor")]
        public string BadgeGradientEndColor { get; set; }
        [DataMember(Name = "badgeGradientStartColor")]
        [JsonProperty(PropertyName = "badgeGradientStartColor")]
        public string BadgeGradientStartColor { get; set; }
        [DataMember(Name = "badgeType")]
        [JsonProperty(PropertyName = "badgeType")]
        public string BadgeType { get; set; }
        [DataMember(Name = "category")]
        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }
        [DataMember(Name = "cheers")]
        [JsonProperty(PropertyName = "cheers")]
        public IList<object> Cheers { get; set; }
        [DataMember(Name = "dateTime")]
        [JsonProperty(PropertyName = "dateTime")]
        public string DateTime { get; set; }
        [DataMember(Name = "description")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [DataMember(Name = "earnedMessage")]
        [JsonProperty(PropertyName = "earnedMessage")]
        public string EarnedMessage { get; set; }
        [DataMember(Name = "encodedId")]
        [JsonProperty(PropertyName = "encodedId")]
        public string EncodedId { get; set; }
        [DataMember(Name = "image100px")]
        [JsonProperty(PropertyName = "image100px")]
        public string Image100px { get; set; }
        [DataMember(Name = "image125px")]
        [JsonProperty(PropertyName = "image125px")]
        public string Image125px { get; set; }
        [DataMember(Name = "image300px")]
        [JsonProperty(PropertyName = "image300px")]
        public string Image300px { get; set; }
        [DataMember(Name = "image50px")]
        [JsonProperty(PropertyName = "image50px")]
        public string Image50px { get; set; }
        [DataMember(Name = "image75px")]
        [JsonProperty(PropertyName = "image75px")]
        public string Image75px { get; set; }
        [DataMember(Name = "marketingDescription")]
        [JsonProperty(PropertyName = "marketingDescription")]
        public string MarketingDescription { get; set; }
        [DataMember(Name = "mobileDescription")]
        [JsonProperty(PropertyName = "mobileDescription")]
        public string MobileDescription { get; set; }
        [DataMember(Name = "name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [DataMember(Name = "shareImage640px")]
        [JsonProperty(PropertyName = "shareImage640px")]
        public string ShareImage640px { get; set; }
        [DataMember(Name = "shareText")]
        [JsonProperty(PropertyName = "shareText")]
        public string ShareText { get; set; }
        [DataMember(Name = "shortDescription")]
        [JsonProperty(PropertyName = "shortDescription")]
        public string ShortDescription { get; set; }
        [DataMember(Name = "shortName")]
        [JsonProperty(PropertyName = "shortName")]
        public string ShortName { get; set; }
        [DataMember(Name = "timesAchieved")]
        [JsonProperty(PropertyName = "timesAchieved")]
        public int TimesAchieved { get; set; }
        [DataMember(Name = "value")]
        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }
        [DataMember(Name = "unit")]
        [JsonProperty(PropertyName = "unit")]
        public string Unit { get; set; }
    }

    [DataContract]
    public class FitbitUser
    {
        [DataMember(Name = "age")]
        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }
        [DataMember(Name = "avatar")]
        [JsonProperty(PropertyName = "avatar")]
        public string Avatar { get; set; }
        [DataMember(Name = "avatar150")]
        [JsonProperty(PropertyName = "avatar150")]
        public string Avatar150 { get; set; }
        [DataMember(Name = "averageDailySteps")]
        [JsonProperty(PropertyName = "averageDailySteps")]
        public int AverageDailySteps { get; set; }
        [DataMember(Name = "clockTimeDisplayFormat")]
        [JsonProperty(PropertyName = "clockTimeDisplayFormat")]
        public string ClockTimeDisplayFormat { get; set; }
        [DataMember(Name = "corporate")]
        [JsonProperty(PropertyName = "corporate")]
        public bool Corporate { get; set; }
        [DataMember(Name = "corporateAdmin")]
        [JsonProperty(PropertyName = "corporateAdmin")]
        public bool CorporateAdmin { get; set; }
        [DataMember(Name = "dateOfBirth")]
        [JsonProperty(PropertyName = "dateOfBirth")]
        public string DateOfBirth { get; set; }
        [DataMember(Name = "displayName")]
        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }
        [DataMember(Name = "distanceUnit")]
        [JsonProperty(PropertyName = "distanceUnit")]
        public string DistanceUnit { get; set; }
        [DataMember(Name = "encodedId")]
        [JsonProperty(PropertyName = "encodedId")]
        public string EncodedId { get; set; }
        [DataMember(Name = "features")]
        [JsonProperty(PropertyName = "features")]
        public FitbitFeatures Features { get; set; }
        [DataMember(Name = "fullName")]
        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }
        [DataMember(Name = "gender")]
        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }
        [DataMember(Name = "glucoseUnit")]
        [JsonProperty(PropertyName = "glucoseUnit")]
        public string GlucoseUnit { get; set; }
        [DataMember(Name = "height")]
        [JsonProperty(PropertyName = "height")]
        public double Height { get; set; }
        [DataMember(Name = "heightUnit")]
        [JsonProperty(PropertyName = "heightUnit")]
        public string HeightUnit { get; set; }
        [DataMember(Name = "locale")]
        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; set; }
        [DataMember(Name = "memberSince")]
        [JsonProperty(PropertyName = "memberSince")]
        public string MemberSince { get; set; }
        [DataMember(Name = "offsetFromUTCMillis")]
        [JsonProperty(PropertyName = "offsetFromUTCMillis")]
        public int OffsetFromUTCMillis { get; set; }
        [DataMember(Name = "startDayOfWeek")]
        [JsonProperty(PropertyName = "startDayOfWeek")]
        public string StartDayOfWeek { get; set; }
        [DataMember(Name = "strideLengthRunning")]
        [JsonProperty(PropertyName = "strideLengthRunning")]
        public double StrideLengthRunning { get; set; }
        [DataMember(Name = "strideLengthRunningType")]
        [JsonProperty(PropertyName = "strideLengthRunningType")]
        public string StrideLengthRunningType { get; set; }
        [DataMember(Name = "strideLengthWalking")]
        [JsonProperty(PropertyName = "strideLengthWalking")]
        public int StrideLengthWalking { get; set; }
        [DataMember(Name = "strideLengthWalkingType")]
        [JsonProperty(PropertyName = "strideLengthWalkingType")]
        public string StrideLengthWalkingType { get; set; }
        [DataMember(Name = "timezone")]
        [JsonProperty(PropertyName = "timezone")]
        public string Timezone { get; set; }
        [DataMember(Name = "topBadges")]
        [JsonProperty(PropertyName = "topBadges")]
        public IList<FitbitTopBadge> TopBadges { get; set; }
        [DataMember(Name = "weight")]
        [JsonProperty(PropertyName = "weight")]
        public int Weight { get; set; }
        [DataMember(Name = "weightUnit")]
        [JsonProperty(PropertyName = "weightUnit")]
        public string WeightUnit { get; set; }
    }

    [DataContract]
    public class FitbitProfileResponse
    {
        [DataMember(Name = "user")]
        [JsonProperty(PropertyName = "user")]
        public FitbitUser User { get; set; }
    }
}
