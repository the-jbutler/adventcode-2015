#!/usr/bin/env ruby

$lines = Array.new
if not ARGV.empty?
	$fileName = ARGV[0]
	File.open($fileName, "r") do |f|
		f.each_line do |line|
			$lines.push(line.chomp)
		end
	end
else
	ARGF.each_line do |line|
		$lines.push(line.chomp)
	end
end

$encodedTotal = 0
$stringTotal = 0

$lines.each do |line|
	$encodedTotal = $encodedTotal + line.dump.length 
	$stringTotal = $stringTotal + line.length
end

printf "Encoded String-String difference: (%d-%d) = %d", $encodedTotal, $stringTotal, ($encodedTotal-$stringTotal)
