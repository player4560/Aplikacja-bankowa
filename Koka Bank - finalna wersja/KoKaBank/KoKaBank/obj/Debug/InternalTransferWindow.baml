   M S B A M L   `   `   `  ����  KoKaBank%    KoKaBank.InternalTransferWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Przelew wewnetrzny��5   	   $	��400��6$   $	��450��61    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	   .��5      � $��20q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $$��Przelew wewnetrzny w KoKa Bank��6!   $��16(�5      $
��Bold"�6"   $��Center=�5      $��0,0,0,20q�5      �� 5   
   $��1��6   $��Numer klienta odbiorcy:��6!   $��0,0,0,5q�6@   �� 5   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��0,0,0,10q�6;   .+�5      �� -   5         ��Name$ RecipientClientIdTextBox��5      $	��200��68   5      �� 5      -   5        $��Sprawdz��5      $��10,0,0,0q�6'   $
��10,2q�69   5      5       �� 5#   
   -   5         ��Name$ RecipientInfoText��5#      $��3��6/   
����  �$��Italic$�5$   &   $��0,0,0,15q�5%      �� 5(   
   $��4��6   $��Kwota przelewu (PLN):��6!   $��0,0,0,5q�6>   �� 5)   
   -   5        $ AmountTextBox��5)      $��5��6)   $��0,0,0,15q�66   �� 5,   
   $��6��6   $��Opis przelewu:��6!   $��0,0,0,5q�67   �� 5-   
   -   5        $ DescriptionTextBox��5-      $��7��6.   $��60��5.       �� TextWrapping$
 Wrap=�6    �� AcceptsReturn . "	 �� VerticalScrollBarVisibility$
	 Auto=�5/   '   $��0,0,0,20q�50      �� 53   
   $��7��6   $ 
Horizontal=�6"   $��Center=�54      $��0,20,0,0q�62   .+�55      �� -   5        $��Wykonaj przelew��55      $
��20,8q�56      $
��10,0q�6$   
������58      �� 59      -   5        $��Anuluj��59      $
��20,8q�5:      $
��10,0q�6$   
�������5<      5=      5>      5?      