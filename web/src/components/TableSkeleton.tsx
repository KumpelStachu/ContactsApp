export default function TableSkeleton({ rows = 5 }: { rows?: number }) {
	return Array.from({ length: rows }).map((_, i) => (
		<tr key={`skeleton${i}`}>
			<td>
				<div className="skeleton w-24 h-4" />
			</td>
			<td>
				<div className="skeleton w-24 h-4" />
			</td>
			<td>
				<div className="skeleton w-24 h-4" />
			</td>
			<td className="flex gap-4">
				<div className="skeleton w-32 h-8 rounded-lg" />
				<div className="skeleton w-8 h-8 rounded-lg" />
				<div className="skeleton w-8 h-8 rounded-lg" />
			</td>
		</tr>
	))
}
