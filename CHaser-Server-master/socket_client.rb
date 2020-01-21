#! /usr/local/bin/ruby
# coding: utf-8
#********************************************************
# Ruby script to test for socket connection.(Client-side)
#********************************************************
#
require 'socket'

# サーバー接続 OPEN
@sock = TCPSocket.open("127.0.0.1", 2001)

	def send_log(str)
		# ソケットに入力文字列を渡す
		@sock.puts str
		@sock.flush

		# サーバーから返却された文字列を出力
		@sock.gets
	end



# ソケット CLOSE
#@sock.close
