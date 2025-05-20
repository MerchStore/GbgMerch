using GbgMerch.Domain.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.IO; // 👈 behövs för Utf8NameDecoder

namespace GbgMerch.Infrastructure.Serialization;

public class MoneySerializer : SerializerBase<Money>
{
    public override Money Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        context.Reader.ReadStartDocument();

        decimal amount = 0;
        string currency = "";

        var nameDecoder = new Utf8NameDecoder(); // 👈 detta är standard

        while (context.Reader.ReadBsonType() != BsonType.EndOfDocument)
        {
            var name = context.Reader.ReadName(nameDecoder); // 👈 fixen

            if (name == "Amount")
                amount = (decimal)context.Reader.ReadDecimal128();
            else if (name == "Currency")
                currency = context.Reader.ReadString();
            else
                context.Reader.SkipValue();
        }

        context.Reader.ReadEndDocument();

        return new Money(amount, currency);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Money value)
    {
        context.Writer.WriteStartDocument();
        context.Writer.WriteName("Amount");
        context.Writer.WriteDecimal128((Decimal128)value.Amount);
        context.Writer.WriteName("Currency");
        context.Writer.WriteString(value.Currency);
        context.Writer.WriteEndDocument();
    }
}
