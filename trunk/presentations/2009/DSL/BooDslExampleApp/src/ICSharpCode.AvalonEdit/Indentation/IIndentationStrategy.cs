// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <author name="Daniel Grunwald"/>
//     <version>$Revision: 3999 $</version>
// </file>

using ICSharpCode.AvalonEdit.Document;
using System;

namespace ICSharpCode.AvalonEdit.Indentation
{
	/// <summary>
	/// Strategy how the text editor handles indentation when new lines are inserted.
	/// </summary>
	public interface IIndentationStrategy
	{
		/// <summary>
		/// Sets the indentation for the specified line.
		/// Usually this is constructed from the indentation of the previous line.
		/// </summary>
		void IndentLine(DocumentLine line);
		
		/// <summary>
		/// Reindents a set of lines.
		/// </summary>
		void IndentLines(int beginLine, int endLine);
	}
}
