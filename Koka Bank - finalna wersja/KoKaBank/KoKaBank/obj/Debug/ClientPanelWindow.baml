   M S B A M L   `   `   `  ����C  ?KoKaBank, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:KoKaBankKoKaBank  5   	        KoKaBank.ClientPanelWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:KoKaBank     5   	     � Title$  Panel Klienta��5   	   $	��500��6   $	��750��6,   .��5
      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0,0,0,20q�6"   .+�5      �� -   5         ��Name$ WelcomeText��5      $��18(�6-   $
��Bold"�6;   $��0,0,0,10q�6M   �� 5      
����ح� �� CornerRadius$ 5��6,    �� Padding$ 10q�6=   ._�5      �� .+�5      �� $��Saldo konta:��6    $
��Bold"�64   �� 5      -   5        $ BalanceText��5       $��16(�65   $
��Bold"�6C   
����  �6U   5      5      5      �� 5   
   $��1��6   $��Historia transakcji��6    $��0,0,0,20q�6=   .��5      &   System.Windows.Controls.DataGrid  -   5          Name$ TransactionList��5         AutoGenerateColumns .     HeadersVisibility$ Column=�5          CanUserAddRows .  	   CanUserDeleteRows	 .  
   
IsReadOnly
 .    GridLinesVisibility$ 
Horizontal=�5$         AlternatingRowBackground
 �����   Columns 5&      0  *System.Windows.Controls.DataGridTextColumn  5'      ,  &System.Windows.Controls.DataGridColumn   Header$
 Data��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 6i   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   �� *Date+ �� StringFormat$ 
dd.MM.yyyy��6i     5(      $
 Opis��6)   $ * 6W    67   �� *Description+6W     5)      $ Kwota��6)   $	 100 6d    68   �� *Amount+$ N2��6d     5*      $ Saldo po��6)   $	 100 6m    6;   �� *BalanceAfter+$ N2��6m   5,      5-      �� 50   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Center=�51      $��0,10,0,0q�62   .+�54      �� -   5        $��Przelew wewnętrzny��54      $	��5,0q�55      $
��15,8q�56      
������
��� � �$��2q�59      $
��Bold"�5:      5;      �� 5>      -   5        $��Przelew zewnętrzny��5>      $	��5,0q�5?      $
��15,8q�5@      
�������5B      �� 5E      -   5        $��Informacje o koncie��5E      $	��5,0q�5F      $
��15,8q�5G      
����ح�5I      �� 5L      -   5        $��Wyloguj��5L      $	��5,0q�5M      $
��15,8q�5N      
�������5P      5Q      5R      5S      