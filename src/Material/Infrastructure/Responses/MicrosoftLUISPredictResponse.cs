using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class MicrosoftLuisIntentsResult
    {

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "label")]
        public object Label { get; set; }

        [DataMember(Name = "score")]
        public double Score { get; set; }
    }

    [DataContract]
    public class Indeces
    {

        [DataMember(Name = "startToken")]
        public int StartToken { get; set; }

        [DataMember(Name = "endToken")]
        public int EndToken { get; set; }
    }

    [DataContract]
    public class MicrosoftLuisEntitiesResult
    {

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "indeces")]
        public Indeces Indeces { get; set; }

        [DataMember(Name = "word")]
        public string Word { get; set; }

        [DataMember(Name = "color")]
        public string Color { get; set; }

        [DataMember(Name = "isBuiltInExtractor")]
        public bool IsBuiltInExtractor { get; set; }
    }

    [DataContract]
    public class MicrosoftLuisPredictResponse
    {

        [DataMember(Name = "IntentsResults")]
        public IList<MicrosoftLuisIntentsResult> IntentsResults { get; set; }

        [DataMember(Name = "EntitiesResults")]
        public IList<MicrosoftLuisEntitiesResult> EntitiesResults { get; set; }

        [DataMember(Name = "DialogResponse")]
        public object DialogResponse { get; set; }

        [DataMember(Name = "utteranceText")]
        public string UtteranceText { get; set; }

        [DataMember(Name = "tokenizedText")]
        public IList<string> TokenizedText { get; set; }

        [DataMember(Name = "exampleId")]
        public string ExampleId { get; set; }

        [DataMember(Name = "metadata")]
        public object Metadata { get; set; }
    }
}
