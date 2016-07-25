using Newtonsoft.Json;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    public class FitbitFeatures
    {
        [JsonProperty(PropertyName = "exerciseGoal")]
        public bool ExerciseGoal { get; set; }
    }

    public class FitbitTopBadge
    {
        [JsonProperty(PropertyName = "badgeGradientEndColor")]
        public string BadgeGradientEndColor { get; set; }
        [JsonProperty(PropertyName = "badgeGradientStartColor")]
        public string BadgeGradientStartColor { get; set; }
        [JsonProperty(PropertyName = "badgeType")]
        public string BadgeType { get; set; }
        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }
        [JsonProperty(PropertyName = "cheers")]
        public IList<object> Cheers { get; set; }
        [JsonProperty(PropertyName = "dateTime")]
        public string DateTime { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "earnedMessage")]
        public string EarnedMessage { get; set; }
        [JsonProperty(PropertyName = "encodedId")]
        public string EncodedId { get; set; }
        [JsonProperty(PropertyName = "image100px")]
        public string Image100px { get; set; }
        [JsonProperty(PropertyName = "image125px")]
        public string Image125px { get; set; }
        [JsonProperty(PropertyName = "image300px")]
        public string Image300px { get; set; }
        [JsonProperty(PropertyName = "image50px")]
        public string Image50px { get; set; }
        [JsonProperty(PropertyName = "image75px")]
        public string Image75px { get; set; }
        [JsonProperty(PropertyName = "marketingDescription")]
        public string MarketingDescription { get; set; }
        [JsonProperty(PropertyName = "mobileDescription")]
        public string MobileDescription { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "shareImage640px")]
        public string ShareImage640px { get; set; }
        [JsonProperty(PropertyName = "shareText")]
        public string ShareText { get; set; }
        [JsonProperty(PropertyName = "shortDescription")]
        public string ShortDescription { get; set; }
        [JsonProperty(PropertyName = "shortName")]
        public string ShortName { get; set; }
        [JsonProperty(PropertyName = "timesAchieved")]
        public int TimesAchieved { get; set; }
        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }
        [JsonProperty(PropertyName = "unit")]
        public string Unit { get; set; }
    }

    public class FitbitUser
    {
        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }
        [JsonProperty(PropertyName = "avatar")]
        public string Avatar { get; set; }
        [JsonProperty(PropertyName = "avatar150")]
        public string Avatar150 { get; set; }
        [JsonProperty(PropertyName = "averageDailySteps")]
        public int AverageDailySteps { get; set; }
        [JsonProperty(PropertyName = "clockTimeDisplayFormat")]
        public string ClockTimeDisplayFormat { get; set; }
        [JsonProperty(PropertyName = "corporate")]
        public bool Corporate { get; set; }
        [JsonProperty(PropertyName = "corporateAdmin")]
        public bool CorporateAdmin { get; set; }
        [JsonProperty(PropertyName = "dateOfBirth")]
        public string DateOfBirth { get; set; }
        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }
        [JsonProperty(PropertyName = "distanceUnit")]
        public string DistanceUnit { get; set; }
        [JsonProperty(PropertyName = "encodedId")]
        public string EncodedId { get; set; }
        [JsonProperty(PropertyName = "features")]
        public FitbitFeatures Features { get; set; }
        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }
        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }
        [JsonProperty(PropertyName = "glucoseUnit")]
        public string GlucoseUnit { get; set; }
        [JsonProperty(PropertyName = "height")]
        public double Height { get; set; }
        [JsonProperty(PropertyName = "heightUnit")]
        public string HeightUnit { get; set; }
        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; set; }
        [JsonProperty(PropertyName = "memberSince")]
        public string MemberSince { get; set; }
        [JsonProperty(PropertyName = "offsetFromUTCMillis")]
        public int OffsetFromUTCMillis { get; set; }
        [JsonProperty(PropertyName = "startDayOfWeek")]
        public string StartDayOfWeek { get; set; }
        [JsonProperty(PropertyName = "strideLengthRunning")]
        public double StrideLengthRunning { get; set; }
        [JsonProperty(PropertyName = "strideLengthRunningType")]
        public string StrideLengthRunningType { get; set; }
        [JsonProperty(PropertyName = "strideLengthWalking")]
        public int StrideLengthWalking { get; set; }
        [JsonProperty(PropertyName = "strideLengthWalkingType")]
        public string StrideLengthWalkingType { get; set; }
        [JsonProperty(PropertyName = "timezone")]
        public string Timezone { get; set; }
        [JsonProperty(PropertyName = "topBadges")]
        public IList<FitbitTopBadge> TopBadges { get; set; }
        [JsonProperty(PropertyName = "weight")]
        public int Weight { get; set; }
        [JsonProperty(PropertyName = "weightUnit")]
        public string WeightUnit { get; set; }
    }

    public class FitbitProfileResponse
    {
        [JsonProperty(PropertyName = "user")]
        public FitbitUser User { get; set; }
    }
}
