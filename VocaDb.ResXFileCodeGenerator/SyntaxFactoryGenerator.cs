﻿using System.Globalization;
using System.Resources;
using System.Web;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace VocaDb.ResXFileCodeGenerator
{
	[Obsolete]
	public sealed class SyntaxFactoryGenerator : IGenerator
	{
		private NamespaceDeclarationSyntax CreateNamespace(GeneratorOptions options) =>
			NamespaceDeclaration(ParseName(options.CustomToolNamespace ?? options.LocalNamespace))
				.AddUsings(
					UsingDirective(IdentifierName(Constants.SystemGlobalization)),
					UsingDirective(IdentifierName(Constants.SystemResources)));

		private ClassDeclarationSyntax CreateClass(GeneratorOptions options) => ClassDeclaration(options.ClassName)
			.AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
			.AddMembers(
				FieldDeclaration(VariableDeclaration(NullableType(IdentifierName(nameof(ResourceManager)))).AddVariables(VariableDeclarator(Constants.s_resourceManagerVariable)))
					.AddModifiers(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.StaticKeyword)),
				PropertyDeclaration(IdentifierName(nameof(ResourceManager)), Constants.ResourceManagerVariable)
					.AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
						.WithExpressionBody(
							ArrowExpressionClause(
								AssignmentExpression(
									SyntaxKind.CoalesceAssignmentExpression,
									IdentifierName(Constants.s_resourceManagerVariable),
									ObjectCreationExpression(IdentifierName(nameof(ResourceManager)))
										.AddArgumentListArguments(
											Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal($"{options.LocalNamespace}.{options.ClassName}"))),
											Argument(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, TypeOfExpression(IdentifierName(options.ClassName)), IdentifierName(nameof(Type.Assembly))))))))
						.WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
				PropertyDeclaration(NullableType(IdentifierName(nameof(CultureInfo))), Constants.CultureInfoVariable)
					.AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
					.AddAccessorListAccessors(
						AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
						AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken))));

		private MemberDeclarationSyntax CreateMember(string name, string value) => PropertyDeclaration(IdentifierName("string"), name)
			.AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
			.WithExpressionBody(
				ArrowExpressionClause(
					PostfixUnaryExpression(SyntaxKind.SuppressNullableWarningExpression, InvocationExpression(
						MemberAccessExpression(
							SyntaxKind.SimpleMemberAccessExpression,
							IdentifierName(Constants.ResourceManagerVariable),
							IdentifierName(nameof(ResourceManager.GetString)))).AddArgumentListArguments(
								Argument(InvocationExpression(IdentifierName("nameof")).AddArgumentListArguments(
									Argument(IdentifierName(name)))),
								Argument(IdentifierName(Constants.CultureInfoVariable))))))
			.WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
			.WithLeadingTrivia(ParseLeadingTrivia($@"/// <summary>
/// Looks up a localized string similar to {HttpUtility.HtmlEncode(value.Trim().Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n/// "))}.
/// </summary>
"));

		private CompilationUnitSyntax GetCompilationUnit(GeneratorOptions options, IEnumerable<MemberDeclarationSyntax> members) => CompilationUnit()
			.AddMembers(
				CreateNamespace(options)
					.AddMembers(
						CreateClass(options)
							.AddMembers(members.ToArray())))
			.WithLeadingTrivia(ParseLeadingTrivia(Constants.AutoGeneratedHeader).Add(
				Trivia(NullableDirectiveTrivia(Token(SyntaxKind.EnableKeyword), true))))
			.NormalizeWhitespace();

		private CompilationUnitSyntax GenerateInternal(Stream resxStream, GeneratorOptions options) => GetCompilationUnit(options, XDocument.Load(resxStream).Root?
			.Descendants()
			.Where(data => data.Name == "data")
			.Select(data => new KeyValuePair<string, string>(data.Attribute("name")!.Value, data.Descendants("value").First().Value))
			.Select(kv => CreateMember(kv.Key, kv.Value)) ?? Enumerable.Empty<MemberDeclarationSyntax>());

		public string Generate(Stream resxStream, GeneratorOptions options) => GenerateInternal(resxStream, options).ToFullString();
	}
}
