   M S B A M L   `   `   `  ����   WDP    Demo.MainWindow   W SPresentationCore, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     �Name$  WikiDumpParser��$	��185��$	��775��$	��775��$	��185��$	��775��$	��185�� � WindowStyle$ ThreeDBorderWindow=�
������� � Title$ RuWikiDumpParser�� � Icon$ 	/icon.ico�� � 
ResizeMode$ CanMinimize=�$��0�� � WindowStartupLocation$ CenterScreen=�$��./Fonts/#Exo 2)�.��]� $��0,0,10,0q�$	��145��$	��Top=�.+�� -    �Name$ grid1��$	��Top=�$	��170��$��0,0,1,0q�$	��780�� � ColumnDefinitions �� $
��190* ��� $
��411* ��� $	��69* ��� $
��110* �.+��� -    ��Name$ 	btnCancel��$��Стоп��$��./Fonts/#Exo 2)�$��	13,59,0,0q�$��20��$	��Top=�$y�Visible=�$��2��	 � Cursor$
	 Hand��$��2��$
��Left=�$	��130���� -   
 ��Name$
 keyWords��$��
119,30,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$��20��$��4��$
��Left=�$	��625��$��./Fonts/#Exo 2)��� -    ��Name$ 
keyWordsLb��$"��Ключевые слова:��$��	4,26,80,0q�$	��Top=�$��24��$��./Fonts/#Exo 2)��� -   $ btnOpenFile��$ ��Искать в файле��$��./Fonts/#Exo 2)�$��4,89,0,0q�$
	 Hand��$��20��$	��Top=�$
��Left=�$	��100��$y�Visible=�${�False������ �� -   $ btnDownloadFile1��$��Искать��$��./Fonts/#Exo 2)�$��	4,118,0,0q�$
	 Hand�� �� 	ClickMode$ Release=�$
��Left=�$	��100��$��20��$	��Top=�${�False���� -    ��Name$ txtProgress��$	��Top=�$��
14,86,35,0q�$��2��$��2��$��./Fonts/#Exo 2)�� -	    �Name$ progressBar��$��20��$��4,4,0,0q�$	��Top=�$��4��$
��Left=�$	��740�����	#FFE6E6E6�� -
   $ 
countPages��$
��Left=�$��	130,6,0,0q�$
��Wrap=�$	��Top=�$	��110��$��1��$��./Fonts/#Exo 2)��� -   $ timer��$
��Left=�$��
14,112,0,0q�$
��Wrap=�$	��Top=�$��2��$��./Fonts/#Exo 2)��� -   $ chooseSaveFolderBT��$��Сохранить в��$
��Left=�$��4,59,0,0q�$	��100��$��20��$	��Top=�$��./Fonts/#Exo 2)��� -   $
 pathTB��$
��Left=�$��
119,59,0,0q�$
 Wrap=�$	��Top=�$	��466��$��2��$��20�� �� 
IsReadOnly$
 True�� J� Opacity$
 0.99Q�
�������$��./Fonts/#Exo 2)��� -   $
 path2TB��$
��Left=�$��
119,89,0,0q�$
 Wrap=�$	��Top=�$	��466��$��2��$��20��$
 True��
�������$��./Fonts/#Exo 2)�