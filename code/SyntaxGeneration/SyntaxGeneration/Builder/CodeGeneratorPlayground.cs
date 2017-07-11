using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace SyntaxGeneration.Builder
{
    public class CodeGeneratorPlayground
    {
        private readonly SyntaxGenerator _syntaxGenerator;

        public CodeGeneratorPlayground()
        {
            var workspace = new AdhocWorkspace();
            _syntaxGenerator = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);
        }

        public SyntaxNode GenerateSyntaxNode()
        {
            var usingDirectives = _syntaxGenerator.NamespaceImportDeclaration("System");

            var lastNameField = _syntaxGenerator.FieldDeclaration("_lastName",
                _syntaxGenerator.TypeExpression(SpecialType.System_String), Accessibility.Private);

            var firstNameField = _syntaxGenerator.FieldDeclaration("_firstName",
                _syntaxGenerator.TypeExpression(SpecialType.System_String), Accessibility.Private);


            var lastNameProperty = _syntaxGenerator.PropertyDeclaration("LastName",
                _syntaxGenerator.TypeExpression(SpecialType.System_String), Accessibility.Public,
                getAccessorStatements: new[]
                {
                    _syntaxGenerator.ReturnStatement(_syntaxGenerator.IdentifierName("_lastName"))
                }, setAccessorStatements: new[]
                {
                    _syntaxGenerator.AssignmentStatement(_syntaxGenerator.IdentifierName("_lastName"),
                        _syntaxGenerator.IdentifierName("value"))
                });

            var firstNameProperty = _syntaxGenerator.PropertyDeclaration("FirstName",
                _syntaxGenerator.TypeExpression(SpecialType.System_String), Accessibility.Public,
                getAccessorStatements: new[]
                {
                    _syntaxGenerator.ReturnStatement(_syntaxGenerator.IdentifierName("_firstName"))
                }, setAccessorStatements: new[]
                {
                    _syntaxGenerator.AssignmentStatement(_syntaxGenerator.IdentifierName("_firstName"),
                        _syntaxGenerator.IdentifierName("value"))
                });


            var cloneMethodBody = _syntaxGenerator.ReturnStatement(
                _syntaxGenerator.InvocationExpression(_syntaxGenerator.IdentifierName("MemberwiseClone")));

            var cloneMethodDeclaration = _syntaxGenerator.MethodDeclaration("Clone", null, null, null,
                Accessibility.Public,
                DeclarationModifiers.Virtual, new[] {cloneMethodBody});

            var cloneableInterfaceType = _syntaxGenerator.IdentifierName("ICloneable");

            var cloneMethodWithInterfaceType =
                _syntaxGenerator.AsPublicInterfaceImplementation(cloneMethodDeclaration, cloneableInterfaceType);

            var constructorParameters = new[]
            {
                _syntaxGenerator.ParameterDeclaration("LastName",
                    _syntaxGenerator.TypeExpression(SpecialType.System_String)),
                _syntaxGenerator.ParameterDeclaration("FirstName",
                    _syntaxGenerator.TypeExpression(SpecialType.System_String))
            };

            var constructorBody = new[]
            {
                _syntaxGenerator.AssignmentStatement(_syntaxGenerator.IdentifierName("_lastName"),
                    _syntaxGenerator.IdentifierName("LastName")),
                _syntaxGenerator.AssignmentStatement(_syntaxGenerator.IdentifierName("_firstName"),
                    _syntaxGenerator.IdentifierName("FirstName")),
            };

            var constructor = _syntaxGenerator.ConstructorDeclaration("Person", constructorParameters,
                Accessibility.Public,
                statements: constructorBody);

            var members = new[]
            {
                lastNameField, firstNameField, lastNameProperty, firstNameProperty, cloneMethodWithInterfaceType,
                constructor
            };


            var classDefinition = _syntaxGenerator.ClassDeclaration("Person", null, Accessibility.Public,
                DeclarationModifiers.Abstract, null, new[] {cloneableInterfaceType}, members);

            var namespaceDeclaration = _syntaxGenerator.NamespaceDeclaration("MyTpes", classDefinition);

            var newNode = _syntaxGenerator.CompilationUnit(usingDirectives, namespaceDeclaration).NormalizeWhitespace();

            return newNode;
        }

        public SyntaxNode GenerateCode()
        {
            var usingDirectives = _syntaxGenerator.NamespaceImportDeclaration("System");

            var messageField =
                _syntaxGenerator.ParameterDeclaration("message",
                    _syntaxGenerator.TypeExpression(SpecialType.System_String));

            var consoleWriteLine = SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName("Console"),
                name: SyntaxFactory.IdentifierName("WriteLine"));

            var arguments = SyntaxFactory.ArgumentList(
                SyntaxFactory.SeparatedList(
                    new[]
                    {
                        SyntaxFactory.Argument(SyntaxFactory.IdentifierName("message"))
                    }));

            var consoleWriteLineStatement = SyntaxFactory
                .ExpressionStatement(SyntaxFactory.InvocationExpression(consoleWriteLine, arguments))
                .Expression;


            var writeMethodDeclartion = _syntaxGenerator.MethodDeclaration("Write", new[] {messageField}, null, null,
                Accessibility.Public,
                DeclarationModifiers.None, new[] {consoleWriteLineStatement});


            var members = new[]
            {
                writeMethodDeclartion
            };


            var classDefinition = _syntaxGenerator.ClassDeclaration("Writer", null, Accessibility.Public,
                DeclarationModifiers.None, null, null, members);

            var namespaceDeclaration = _syntaxGenerator.NamespaceDeclaration("RoslynCompileSample", classDefinition);

            var newNode = _syntaxGenerator.CompilationUnit(usingDirectives, namespaceDeclaration).NormalizeWhitespace();


            return newNode;
        }

        public SyntaxNode GenerateComplexCode()
        {
            //using statements
            var systemUsingDeclaration = _syntaxGenerator.NamespaceImportDeclaration("System");
            var writeToConsoleUsing = _syntaxGenerator.NamespaceImportDeclaration("SyntaxGeneration.Service");

            //new private generic class field
            var genericClassField = _syntaxGenerator.FieldDeclaration("_genericClass",
                _syntaxGenerator.IdentifierName("GenericClass"), Accessibility.Private, DeclarationModifiers.Static);

            //constructor main code
            var constructorBody = _syntaxGenerator.AssignmentStatement(_syntaxGenerator.IdentifierName("_genericClass"), _syntaxGenerator.IdentifierName("genericClass"));
            //constructor method generation
            var constructor = _syntaxGenerator.ConstructorDeclaration("Program",
                new[]
                {
                    _syntaxGenerator.ParameterDeclaration("genericClass",
                        _syntaxGenerator.IdentifierName("GenericClass"))
                }, Accessibility.Public,
                DeclarationModifiers.None, null, new[] {constructorBody});

            //Fields for use with our main method
            var priceOneField =
                _syntaxGenerator.ParameterDeclaration("priceOne",
                    _syntaxGenerator.TypeExpression(SpecialType.System_Int32));

            var priceTwoField =
                _syntaxGenerator.ParameterDeclaration("priceTwo",
                    _syntaxGenerator.TypeExpression(SpecialType.System_Int32));

            //Expression for adding the two fields together
            var additionExpression = _syntaxGenerator.AddExpression(
                _syntaxGenerator.IdentifierName("priceOne"),
                _syntaxGenerator.IdentifierName("priceTwo"));

            //total field
            var totalField = _syntaxGenerator.LocalDeclarationStatement("total", additionExpression);

            var consoleWriteLine = _syntaxGenerator.MemberAccessExpression(
                _syntaxGenerator.IdentifierName("_genericClass"),
                _syntaxGenerator.IdentifierName("WritePriceToConsole"));

            var arguements = _syntaxGenerator.Argument(RefKind.None, _syntaxGenerator.IdentifierName("total"));

            var writeLineExpression = _syntaxGenerator.InvocationExpression(consoleWriteLine, arguements);

            var returnStatement =
                _syntaxGenerator.ReturnStatement(
                    _syntaxGenerator.IdentifierName("total"));

            var methodDeclartion = _syntaxGenerator.MethodDeclaration("GetCalculatedPrice",
                new[] {priceOneField, priceTwoField}, null, _syntaxGenerator.TypeExpression(SpecialType.System_Int32),
                Accessibility.Public, DeclarationModifiers.Static,
                new[] {totalField, writeLineExpression, returnStatement});

            var methodName = _syntaxGenerator.IdentifierName("GetCalculatedPrice");

            var calculationArgumentsOne = _syntaxGenerator.Argument(RefKind.None, _syntaxGenerator.LiteralExpression(6));
            var calculationArgumentsTwo = _syntaxGenerator.Argument(RefKind.None, _syntaxGenerator.LiteralExpression(12));

            var methodCall =
                _syntaxGenerator.InvocationExpression(methodName, calculationArgumentsOne, calculationArgumentsTwo);

            var mainMethod = _syntaxGenerator.MethodDeclaration("Main", null, null,
                null, Accessibility.NotApplicable,
                DeclarationModifiers.Static, new[] { methodCall });


            var members = new[] {genericClassField, constructor, mainMethod, methodDeclartion};

            var classDefinition = _syntaxGenerator.ClassDeclaration("Program", null, Accessibility.Public,
                DeclarationModifiers.None, null, null, members);

            var nameSpaceDecleration = _syntaxGenerator.NamespaceDeclaration("MyNameSpace", classDefinition);

            var newNode = _syntaxGenerator
                .CompilationUnit(systemUsingDeclaration, writeToConsoleUsing, nameSpaceDecleration)
                .NormalizeWhitespace();

            return newNode;
        }
    }
}