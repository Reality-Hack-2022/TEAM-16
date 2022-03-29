using System.Collections;
using System.Runtime;
using System.Collections.Generic;
using BERTTokenizers;
using UnityEngine;

public class bertTest : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
		var sentence = "I love you";

		var tokenizer = new BertBaseTokenizer();

		var encoded = tokenizer.Encode(256, sentence);

        // var bertInput = new BertInput()
        // {
        //     InputIds = encoded.Select(t => t.InputIds).ToArray(),
        //     AttentionMask = encoded.Select(t => t.AttentionMask).ToArray(),
        //     TypeIds = encoded.Select(t => t.TokenTypeIds).ToArray()
        // };
        // Debug.Log(bertInput);
    }

}
//public class BertInput
//{
//    [VectorType(1, 256)]
//    [ColumnName("input_ids")]
//    public long[] InputIds { get; set; }

//    [VectorType(1, 256)]
//    [ColumnName("attention_mask")]
//    public long[] AttentionMask { get; set; }

//    [VectorType(1, 256)]
//    [ColumnName("token_type_ids")]
//    public long[] TypeIds { get; set; }
//}