using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FitbitFeatures
    {
        [DataMember(Name = "exerciseGoal")]
        public bool ExerciseGoal { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FitbitTopBadge
    {
        [DataMember(Name = "badgeGradientEndColor")]
        public string BadgeGradientEndColor { get; set; }
        [DataMember(Name = "badgeGradientStartColor")]
        public string BadgeGradientStartColor { get; set; }
        [DataMember(Name = "badgeType")]
        public string BadgeType { get; set; }
        [DataMember(Name = "category")]
        public string Category { get; set; }
        [DataMember(Name = "cheers")]
        public IList<object> Cheers { get; set; }
        [DataMember(Name = "dateTime")]
        public string DateTime { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "earnedMessage")]
        public string EarnedMessage { get; set; }
        [DataMember(Name = "encodedId")]
        public string EncodedId { get; set; }
        [DataMember(Name = "image100px")]
        public string Image100px { get; set; }
        [DataMember(Name = "image125px")]
        public string Image125px { get; set; }
        [DataMember(Name = "image300px")]
        public string Image300px { get; set; }
        [DataMember(Name = "image50px")]
        public string Image50px { get; set; }
        [DataMember(Name = "image75px")]
        public string Image75px { get; set; }
        [DataMember(Name = "marketingDescription")]
        public string MarketingDescription { get; set; }
        [DataMember(Name = "mobileDescription")]
        public string MobileDescription { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "shareImage640px")]
        public string ShareImage640px { get; set; }
        [DataMember(Name = "shareText")]
        public string ShareText { get; set; }
        [DataMember(Name = "shortDescription")]
        public string ShortDescription { get; set; }
        [DataMember(Name = "shortName")]
        public string ShortName { get; set; }
        [DataMember(Name = "timesAchieved")]
        public int TimesAchieved { get; set; }
        [DataMember(Name = "value")]
        public int Value { get; set; }
        [DataMember(Name = "unit")]
        public string Unit { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FitbitUser
    {
        [DataMember(Name = "age")]
        public int Age { get; set; }
        [DataMember(Name = "avatar")]
        public string Avatar { get; set; }
        [DataMember(Name = "avatar150")]
        public string Avatar150 { get; set; }
        [DataMember(Name = "averageDailySteps")]
        public int AverageDailySteps { get; set; }
        [DataMember(Name = "clockTimeDisplayFormat")]
        public string ClockTimeDisplayFormat { get; set; }
        [DataMember(Name = "corporate")]
        public bool Corporate { get; set; }
        [DataMember(Name = "corporateAdmin")]
        public bool CorporateAdmin { get; set; }
        [DataMember(Name = "dateOfBirth")]
        public string DateOfBirth { get; set; }
        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }
        [DataMember(Name = "distanceUnit")]
        public string DistanceUnit { get; set; }
        [DataMember(Name = "encodedId")]
        public string EncodedId { get; set; }
        [DataMember(Name = "features")]
        public FitbitFeatures Features { get; set; }
        [DataMember(Name = "fullName")]
        public string FullName { get; set; }
        [DataMember(Name = "gender")]
        public string Gender { get; set; }
        [DataMember(Name = "glucoseUnit")]
        public string GlucoseUnit { get; set; }
        [DataMember(Name = "height")]
        public double Height { get; set; }
        [DataMember(Name = "heightUnit")]
        public string HeightUnit { get; set; }
        [DataMember(Name = "locale")]
        public string Locale { get; set; }
        [DataMember(Name = "memberSince")]
        public string MemberSince { get; set; }
        [DataMember(Name = "offsetFromUTCMillis")]
        public int OffsetFromUTCMillis { get; set; }
        [DataMember(Name = "startDayOfWeek")]
        public string StartDayOfWeek { get; set; }
        [DataMember(Name = "strideLengthRunning")]
        public double StrideLengthRunning { get; set; }
        [DataMember(Name = "strideLengthRunningType")]
        public string StrideLengthRunningType { get; set; }
        [DataMember(Name = "strideLengthWalking")]
        public int StrideLengthWalking { get; set; }
        [DataMember(Name = "strideLengthWalkingType")]
        public string StrideLengthWalkingType { get; set; }
        [DataMember(Name = "timezone")]
        public string Timezone { get; set; }
        [DataMember(Name = "topBadges")]
        public IList<FitbitTopBadge> TopBadges { get; set; }
        [DataMember(Name = "weight")]
        public int Weight { get; set; }
        [DataMember(Name = "weightUnit")]
        public string WeightUnit { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FitbitProfileResponse
    {
        [DataMember(Name = "user")]
        public FitbitUser User { get; set; }
    }
}
