   M S B A M L   `   `   `  ����D  @Obfuscate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Obfuscate	Obfuscate  5   	       Obfuscate.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:Obfuscate     5   	   -   5          �Name$  
Obfuscator��5   	    � Title$ 
MainWindow��5   	   $	��550��6   $	��900��6)    � 
ResizeMode$ NoResize=�65   
�������.��5	      ]� ��. .+�5
   
   �� $	��Top=�6   
�������$��2q�69   ._�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� .+�5      �� -   5         ��Name$ 	btnBrowse��5      $��5q�60   $��Center=�6;   $��Center=�6]   6�   �� 5      $��Center=�6    $��5q�6=   /-Для начала выберите файл6b   �� 5      -   5        $ btnObfuscate��5      $��5q�63   $��Center=�6>   $��Center=�6`   6�   5      �� 5      -   5         ��Name$ tbxProcesses��5      $��Stretch=�60   $
��Auto=�6N   $
��Auto=�6�   $��Stretch=�6�   $	��750��6�   $	��100��6�    �� 
IsReadOnly . 6�   5      5      �� 5   
   $��Stretch=�6   .+�5      �� $��5q�6   Исходный код61   �� 5      -   5        $ tbxCode��5      $��5q�6'   $��Stretch=�62   $	��130��6P    . $
��Auto=�6o   $
��Auto=�6�   6�   �� 5      $��5q�6   '%Обфусцированный код68   �� 5      -   5        $ tbxObfuscatedCode��5      $��5q�61   $��Stretch=�6<   $	��130��6Z    . $
��Auto=�6y   $
��Auto=�6�   6�   �� 5      $��Center=�6   ._�5      �� $��Center=�6   .+�5      �� -   5        $ btnSave��5      $��Right=�6.   $	��100��6J   $��25��6V   $��Center=�6b   $��20,15,50,10q�6}   .��6�   Сохранить6�   5      5      5      5      5       