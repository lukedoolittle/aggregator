using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class FatsecretFoodEntry
    {
        [DataMember(Name = "calcium")]
        [JsonProperty(PropertyName = "calcium")]
        public string Calcium { get; set; }
        [DataMember(Name = "calories")]
        [JsonProperty(PropertyName = "calories")]
        public string Calories { get; set; }
        [DataMember(Name = "carbohydrate")]
        [JsonProperty(PropertyName = "carbohydrate")]
        public string Carbohydrate { get; set; }
        [DataMember(Name = "cholesterol")]
        [JsonProperty(PropertyName = "cholesterol")]
        public string Cholesterol { get; set; }
        [DataMember(Name = "date_int")]
        [JsonProperty(PropertyName = "date_int")]
        public string DateInt { get; set; }
        [DataMember(Name = "fat")]
        [JsonProperty(PropertyName = "fat")]
        public string Fat { get; set; }
        [DataMember(Name = "fiber")]
        [JsonProperty(PropertyName = "fiber")]
        public string Fiber { get; set; }
        [DataMember(Name = "food_entry_description")]
        [JsonProperty(PropertyName = "food_entry_description")]
        public string FoodEntryDescription { get; set; }
        [DataMember(Name = "food_entry_id")]
        [JsonProperty(PropertyName = "food_entry_id")]
        public string FoodEntryId { get; set; }
        [DataMember(Name = "food_entry_name")]
        [JsonProperty(PropertyName = "food_entry_name")]
        public string FoodEntryName { get; set; }
        [DataMember(Name = "food_id")]
        [JsonProperty(PropertyName = "food_id")]
        public string FoodId { get; set; }
        [DataMember(Name = "iron")]
        [JsonProperty(PropertyName = "iron")]
        public string Iron { get; set; }
        [DataMember(Name = "meal")]
        [JsonProperty(PropertyName = "meal")]
        public string Meal { get; set; }
        [DataMember(Name = "monounsaturated_fat")]
        [JsonProperty(PropertyName = "monounsaturated_fat")]
        public string MonounsaturatedFat { get; set; }
        [DataMember(Name = "number_of_units")]
        [JsonProperty(PropertyName = "number_of_units")]
        public string NumberOfUnits { get; set; }
        [DataMember(Name = "polyunsaturated_fat")]
        [JsonProperty(PropertyName = "polyunsaturated_fat")]
        public string PolyunsaturatedFat { get; set; }
        [DataMember(Name = "potassium")]
        [JsonProperty(PropertyName = "potassium")]
        public string Potassium { get; set; }
        [DataMember(Name = "protein")]
        [JsonProperty(PropertyName = "protein")]
        public string Protein { get; set; }
        [DataMember(Name = "saturated_fat")]
        [JsonProperty(PropertyName = "saturated_fat")]
        public string SaturatedFat { get; set; }
        [DataMember(Name = "serving_id")]
        [JsonProperty(PropertyName = "serving_id")]
        public string ServingId { get; set; }
        [DataMember(Name = "sodium")]
        [JsonProperty(PropertyName = "sodium")]
        public string Sodium { get; set; }
        [DataMember(Name = "sugar")]
        [JsonProperty(PropertyName = "sugar")]
        public string Sugar { get; set; }
        [DataMember(Name = "trans_fat")]
        [JsonProperty(PropertyName = "trans_fat")]
        public string TransFat { get; set; }
        [DataMember(Name = "vitamin_a")]
        [JsonProperty(PropertyName = "vitamin_a")]
        public string VitaminA { get; set; }
        [DataMember(Name = "vitamin_c")]
        [JsonProperty(PropertyName = "vitamin_c")]
        public string VitaminC { get; set; }
    }

    [DataContract]
    public class FatsecretFoodEntries
    {
        [DataMember(Name = "food_entry")]
        [JsonProperty(PropertyName = "food_entry")]
        public IList<FatsecretFoodEntry> FoodEntry { get; set; }
    }

    [DataContract]
    public class FatsecretMealResponse
    {
        [DataMember(Name = "food_entries")]
        [JsonProperty(PropertyName = "food_entries")]
        public FatsecretFoodEntries FoodEntries { get; set; }
    }


}
