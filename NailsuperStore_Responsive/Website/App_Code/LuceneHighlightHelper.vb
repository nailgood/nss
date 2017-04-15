
Imports System.Collections.Generic
Imports System.IO

Imports Lucene.Net.Analysis
Imports Lucene.Net.Analysis.Standard
Imports Lucene.Net.QueryParsers
Imports Lucene.Net.Search
Imports Lucene.Net.Highlight


Public Class LuceneHighlightHelper
    Private ReadOnly _luceneVersion As Lucene.Net.Util.Version = Lucene.Net.Util.Version.LUCENE_29

    Protected QueryParsers As New Dictionary(Of String, QueryParser)()

    Public Property Separator() As String
        Get
            Return m_Separator
        End Get
        Set(value As String)
            m_Separator = value
        End Set
    End Property
    Private m_Separator As String
    Public Property MaxNumHighlights() As Integer
        Get
            Return m_MaxNumHighlights
        End Get
        Set(value As Integer)
            m_MaxNumHighlights = value
        End Set
    End Property
    Private m_MaxNumHighlights As Integer
    Public Property HighlightFormatter() As Formatter
        Get
            Return m_HighlightFormatter
        End Get
        Set(value As Formatter)
            m_HighlightFormatter = value
        End Set
    End Property
    Private m_HighlightFormatter As Formatter
    Public Property HighlightAnalyzer() As Analyzer
        Get
            Return m_HighlightAnalyzer
        End Get
        Set(value As Analyzer)
            m_HighlightAnalyzer = value
        End Set
    End Property
    Private m_HighlightAnalyzer As Analyzer

    Private Shared ReadOnly m_instance As New LuceneHighlightHelper()

    Public Shared ReadOnly Property Instance() As LuceneHighlightHelper
        Get
            Return m_instance
        End Get
    End Property

    Private Sub New()
        Separator = "..."
        MaxNumHighlights = 5
        HighlightAnalyzer = New StandardAnalyzer(_luceneVersion)
        HighlightFormatter = New SimpleHTMLFormatter("<span class='highlightKeyword'>", "</span>")
    End Sub


    Public Function GetHighlight(value As String, highlightField As String, searcher As IndexSearcher, luceneRawQuery As String) As String
        Dim query = GetQueryParser(highlightField).Parse(luceneRawQuery)
        Dim scorer = New QueryScorer(query.Rewrite(searcher.GetIndexReader()))

        Dim highlighter = New Highlighter(HighlightFormatter, scorer)

        Dim tokenStream = HighlightAnalyzer.TokenStream(highlightField, New StringReader(value))
        Return highlighter.GetBestFragments(tokenStream, value, MaxNumHighlights, Separator)
    End Function

    Public Function GetHighlight(value As String, searcher As IndexSearcher, highlightField As String, luceneQuery As Query) As String
        Dim scorer = New QueryScorer(luceneQuery.Rewrite(searcher.GetIndexReader()))
        Dim highlighter = New Highlighter(HighlightFormatter, scorer)

        Dim tokenStream = HighlightAnalyzer.TokenStream(highlightField, New StringReader(value))
        Return highlighter.GetBestFragments(tokenStream, value, MaxNumHighlights, Separator)
    End Function

    Protected Function GetQueryParser(highlightField As String) As QueryParser
        If Not QueryParsers.ContainsKey(highlightField) Then
            QueryParsers(highlightField) = New QueryParser(_luceneVersion, highlightField, HighlightAnalyzer)
        End If
        Return QueryParsers(highlightField)
    End Function
End Class

