﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.VisualStudio.LanguageServices.Implementation.ChangeSignature;
using static Microsoft.VisualStudio.LanguageServices.Implementation.ChangeSignature.ChangeSignatureDialogViewModel;

namespace Microsoft.VisualStudio.LanguageServices.CSharp.ChangeSignature
{
    [ExportLanguageService(typeof(IChangeSignatureViewModelFactoryService), LanguageNames.CSharp), Shared]
    internal class CSharpChangeSignatureViewModelFactoryService : ChangeSignatureViewModelFactoryService
    {
        [ImportingConstructor]
        [Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
        public CSharpChangeSignatureViewModelFactoryService()
        {
        }

        public override SymbolDisplayPart[] GeneratePreviewDisplayParts(AddedParameterViewModel addedParameterViewModel)
        {
            var parts = new List<SymbolDisplayPart>();

            var isPredefinedType = SyntaxFactory.ParseExpression(addedParameterViewModel.TypeWithoutErrorIndicator).Kind() == SyntaxKind.PredefinedType;
            var typePartKind = isPredefinedType ? SymbolDisplayPartKind.Keyword : SymbolDisplayPartKind.ClassName;

            parts.Add(new SymbolDisplayPart(typePartKind, null, addedParameterViewModel.TypeWithoutErrorIndicator));
            parts.Add(new SymbolDisplayPart(SymbolDisplayPartKind.Space, null, " "));
            parts.Add(new SymbolDisplayPart(SymbolDisplayPartKind.ParameterName, null, addedParameterViewModel.ParameterName));

            if (!string.IsNullOrWhiteSpace(addedParameterViewModel.Default))
            {
                parts.Add(new SymbolDisplayPart(SymbolDisplayPartKind.Space, null, " "));
                parts.Add(new SymbolDisplayPart(SymbolDisplayPartKind.Punctuation, null, "="));
                parts.Add(new SymbolDisplayPart(SymbolDisplayPartKind.Space, null, " "));
                parts.Add(new SymbolDisplayPart(SymbolDisplayPartKind.Text, null, addedParameterViewModel.Default));
            }

            return parts.ToArray();
        }

        public override bool IsTypeNameValid(string typeName) => !SyntaxFactory.ParseTypeName(typeName).ContainsDiagnostics;

        public override SyntaxNode GetTypeNode(string typeName) => SyntaxFactory.ParseTypeName(typeName);
    }
}
