// The MIT License (MIT)
// 
// Copyright (c) 2022 Shiny Dolphin Games LLC
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.IO;
using System.Linq;
using UnityEngine;

public class GitInfo
{
	public static string FindGitHash() {
		var headRef = File.ReadAllText(Path.Combine(Application.dataPath, "..", ".git", "HEAD")).Replace("ref: ", "");

		var finalPath = new[] { Application.dataPath, "..", ".git" }
		  .Concat(headRef.Split('/'))
		  .Select(StripCharacters)
		  .ToArray();

		var gitCommit = StripCharacters(File.ReadAllText(Path.Combine(finalPath)));
		return gitCommit;
	}

	public static string FindGitShortHash() => FindGitHash().Substring(0, 7);

	static string StripCharacters(string orig) => Path.GetInvalidPathChars().Aggregate(orig, (curr, invalid) => curr.Replace(invalid.ToString(), ""));
}
