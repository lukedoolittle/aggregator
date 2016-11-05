using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FatsecretFoodEntry
    {
        [DataMember(Name = "calcium")]
        public string Calcium { get; set; }
        [DataMember(Name = "calories")]
        public string Calories { get; set; }
        [DataMember(Name = "carbohydrate")]
        public string Carbohydrate { get; set; }
        [DataMember(Name = "cholesterol")]
        public string Cholesterol { get; set; }
        [DataMember(Name = "date_int")]
        public string DateInt { get; set; }
        [DataMember(Name = "fat")]
        public string Fat { get; set; }
        [DataMember(Name = "fiber")]
        public string Fiber { get; set; }
        [DataMember(Name = "food_entry_description")]
        public string FoodEntryDescription { get; set; }
        [DataMember(Name = "food_entry_id")]
        public string FoodEntryId { get; set; }
        [DataMember(Name = "food_entry_name")]
        public string FoodEntryName { get; set; }
        [DataMember(Name = "food_id")]
        public string FoodId { get; set; }
        [DataMember(Name = "iron")]
        public string Iron { get; set; }
        [DataMember(Name = "meal")]
        public string Meal { get; set; }
        [DataMember(Name = "monounsaturated_fat")]
        public string MonounsaturatedFat { get; set; }
        [DataMember(Name = "number_of_units")]
        public string NumberOfUnits { get; set; }
        [DataMember(Name = "polyunsaturated_fat")]
        public string PolyunsaturatedFat { get; set; }
        [DataMember(Name = "potassium")]
        public string Potassium { get; set; }
        [DataMember(Name = "protein")]
        public string Protein { get; set; }
        [DataMember(Name = "saturated_fat")]
        public string SaturatedFat { get; set; }
        [DataMember(Name = "serving_id")]
        public string ServingId { get; set; }
        [DataMember(Name = "sodium")]
        public string Sodium { get; set; }
        [DataMember(Name = "sugar")]
        public string Sugar { get; set; }
        [DataMember(Name = "trans_fat")]
        public string TransFat { get; set; }
        [DataMember(Name = "vitamin_a")]
        public string VitaminA { get; set; }
        [DataMember(Name = "vitamin_c")]
        public string VitaminC { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FatsecretFoodEntries
    {
        [DataMember(Name = "food_entry")]
        public IList<FatsecretFoodEntry> FoodEntry { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FatsecretMealResponse
    {
        [DataMember(Name = "food_entries")]
        public FatsecretFoodEntries FoodEntries { get; set; }
    }
}
