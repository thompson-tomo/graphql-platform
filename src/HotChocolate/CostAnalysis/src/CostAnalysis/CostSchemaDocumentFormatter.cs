using HotChocolate.Language;

namespace HotChocolate.CostAnalysis;

internal sealed class CostSchemaDocumentFormatter(ISchemaDefinition schema) : ISchemaDocumentFormatter
{
    public DocumentNode Format(DocumentNode schemaDocument)
    {
        var rewriter = new CostSyntaxRewriter();
        return (DocumentNode)rewriter.Rewrite(schemaDocument, new CostSyntaxRewriter.Context(schema))!;
    }
}
