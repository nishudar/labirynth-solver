namespace LabyrinthCore.Data;

public static class FieldMapper
{
    public static char ToFieldChar(this FieldType fieldType)
    {
        return fieldType switch
        {
            FieldType.Empty => ' ',
            FieldType.End => 'E',
            FieldType.Start => 'S',
            FieldType.Wall => 'X',
            FieldType.Teleport => 'T',
            FieldType.Door => 'D',
            FieldType.Key => 'K',
            _ => ' '
        };
    }

    public static FieldType ToFieldType(this char fieldChar)
    {
        return fieldChar switch
        {
            ' ' => FieldType.Empty,
            'E' => FieldType.End,
            'S' => FieldType.Start,
            'X' => FieldType.Wall,
            'T' => FieldType.Teleport,
            'K' => FieldType.Key,
            'D' => FieldType.Door,
            _ => throw new ArgumentOutOfRangeException(nameof(fieldChar), fieldChar, null)
        };
    }
    
    
}