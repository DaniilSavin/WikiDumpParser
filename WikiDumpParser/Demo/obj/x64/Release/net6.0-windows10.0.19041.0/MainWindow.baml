   M S B A M L   `   `   `  ����   WDP    Demo.MainWindow   W SPresentationCore, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     �Name$  WikiDumpParser��$	��170��$	��780��$	��780��$	��190��$	��780��$	��190�� � WindowStyle$ ThreeDBorderWindow=�
������� � Title$ WikiDumpParser�� � Icon$ 	/icon.ico�� � 
ResizeMode$ CanMinimize=�$��0�� � WindowStartupLocation$ CenterScreen=�.��]� $��0,0,10,0q�$	��145��$	��Top=�.+�� -    �Name$ grid1��$	��Top=�$	��170��$��0,0,1,0q�$	��780�� � ColumnDefinitions �� $
��181* ��� $
��459* ��� $
��105* �.+��� -    ��Name$ 	btnCancel��$��Стоп��$��
425,59,0,0q�$��20��$	��Top=�$y�Visible=�$��2��	 � Cursor$
	 Hand��$��1��$
��Left=�$	��130���� -   
 ��Name$
 keyWords��$��
119,30,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$��18��$��3��$
��Left=�$	��626���� -    ��Name$ 
keyWordsLb��$"��Ключевые слова:��$��	4,24,62,0q�$	��Top=�$��24���� -   $ btnOpenFile��$ ��Искать в файле��$��4,89,0,0q�$
	 Hand��$��20��$	��Top=�$
��Left=�$	��116��$y�Visible=�${�False������ �� -   $ btnDownloadFile1��$��Искать��$��	4,118,0,0q�$
	 Hand�� �� 	ClickMode$ Release=�$
��Left=�$	��116��$��20��$	��Top=�${�False���� -    ��Name$ txtProgress��$	��Top=�$��425,86,35,0q�$��2��$��1��� -	    �Name$ progressBar��$��16��$��4,4,0,0q�$	��Top=� � Value$ 1Q� � IsIndeterminate$
 True��$��3��$
��Left=�$	��741�����	#FFE6E6E6�� -
   $ 
countPages��$
��Left=�$��	170,4,0,0q�$
��Wrap=�$	��Top=�$	��110��$��1���� -   $ timer��$
��Left=�$��425,112,0,0q�$
��Wrap=�$	��Top=�$��1���� -   $ chooseSaveFolderBT��$��Сохранить в��$
��Left=�$��4,59,0,0q�$	��116��$��20��$	��Top=��� -   $ pathTB��$��
129,61,0,0q�$
��Wrap=�$	��Top=� J� RenderTransformOrigin$ 0.447,0.523&�$
��Left=�$	��466��$��2��