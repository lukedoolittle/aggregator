using Newtonsoft.Json;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    public class FatsecretFoodEntry
    {
        [JsonProperty(PropertyName = "calcium")]
        public string Calcium { get; set; }
        [JsonProperty(PropertyName = "calories")]
        public string Calories { get; set; }
        [JsonProperty(PropertyName = "carbohydrate")]
        public string Carbohydrate { get; set; }
        [JsonProperty(PropertyName = "cholesterol")]
        public string Cholesterol { get; set; }
        [JsonProperty(PropertyName = "date_int")]
        public string DateInt { get; set; }
        [JsonProperty(PropertyName = "fat")]
        public string Fat { get; set; }
        [JsonProperty(PropertyName = "fiber")]
        public string Fiber { get; set; }
        [JsonProperty(PropertyName = "food_entry_description")]
        public string FoodEntryDescription { get; set; }
        [JsonProperty(PropertyName = "food_entry_id")]
        public string FoodEntryId { get; set; }
        [JsonProperty(PropertyName = "food_entry_name")]
        public string FoodEntryName { get; set; }
        [JsonProperty(PropertyName = "food_id")]
        public string FoodId { get; set; }
        [JsonProperty(PropertyName = "iron")]
        public string Iron { get; set; }
        [JsonProperty(PropertyName = "meal")]
        public string Meal { get; set; }
        [JsonProperty(PropertyName = "monounsaturated_fat")]
        public string MonounsaturatedFat { get; set; }
        [JsonProperty(PropertyName = "number_of_units")]
        public string NumberOfUnits { get; set; }
        [JsonProperty(PropertyName = "polyunsaturated_fat")]
        public string PolyunsaturatedFat { get; set; }
        [JsonProperty(PropertyName = "potassium")]
        public string Potassium { get; set; }
        [JsonProperty(PropertyName = "protein")]
        public string Protein { get; set; }
        [JsonProperty(PropertyName = "saturated_fat")]
        public string SaturatedFat { get; set; }
        [JsonProperty(PropertyName = "serving_id")]
        public string ServingId { get; set; }
        [JsonProperty(PropertyName = "sodium")]
        public string Sodium { get; set; }
        [JsonProperty(PropertyName = "sugar")]
        public string Sugar { get; set; }
        [JsonProperty(PropertyName = "trans_fat")]
        public string TransFat { get; set; }
        [JsonProperty(PropertyName = "vitamin_a")]
        public string VitaminA { get; set; }
        [JsonProperty(PropertyName = "vitamin_c")]
        public string VitaminC { get; set; }
    }

    public class FatsecretFoodEntries
    {
        [JsonProperty(PropertyName = "food_entry")]
        public IList<FatsecretFoodEntry> FoodEntry { get; set; }
    }

    public class FatsecretMealResponse
    {
        [JsonProperty(PropertyName = "food_entries")]
        public FatsecretFoodEntries FoodEntries { get; set; }
    }


}
