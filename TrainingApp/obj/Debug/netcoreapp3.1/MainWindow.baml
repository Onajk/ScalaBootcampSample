   M S B A M L   `   `   `  ����  TrainingApp    TrainingApp.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  
MainWindow��5   	   $	��600��6   $	��920��6)    � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5	   	   
������  � AllowsTransparency$
 True��5   	    � WindowStartupLocation$ CenterScreen=�5   	   .��5      �� ���#272537 �� CornerRadius$ 20��5      ._�5   
   �  � ColumnDefinitions 5      �� 5      $	��220 �6#   �� 5      .+�5      �� .+�5      �� $��Paweł's sample code��6   $��Center=�5      $��Center=�5      
�������$��20(�5      $��	0,10,0,50q�5      $��/Fonts/#Poppins)�5      	� 5      $
��Home��6   $��50��5      
�������$��14(�5!         MenuButtonTheme#��[  5"       d� 	IsChecked$
 TrueY�5#      ��5$      �� *HomeViewCommand+	� 5%      $��Tic Tac Toe��6   $��50��5&      
�������$��14(�5(      #��[  5)      ��5*      �� *TicTacToeViewCommand+	� 5+      $��Notes��6   $��50��5,      
�������$��14(�5.      #��[  5/      ��50      �� *NotesViewCommand+	� 51      $��Book finder��6   $��50��52      
�������$��14(�54      #��[  55      ��56      �� *BookFinderViewCommand+57      �� 59      $��1��6   $��Right=�5:      $	��Top=�5;      $��X��5<      
�������$��40(�5>      $��/Fonts/#Poppins)�5?      
������ 
������ $��	0,-5,10,0q�5B      ��5C      �� *AppCloseCommand+�� 5E      $��1��6   $
��left=�5F      $��Bottom=�5G      $��0,0,0,20q�5H      ��5I      �� *CurrentView+5K      5L      5N      