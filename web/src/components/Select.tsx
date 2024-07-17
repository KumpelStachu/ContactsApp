import clsx from 'clsx'
import { LucideIcon } from 'lucide-react'
import { DetailedHTMLProps, SelectHTMLAttributes } from 'react'

export default function Select<Values extends string>({
	data,
	label,
	icon: Icon,
	className,
	labelClassName,
	onSelected,
	...props
}: DetailedHTMLProps<SelectHTMLAttributes<HTMLSelectElement>, HTMLSelectElement> & {
	data: { value: Values; label: string }[]
	label?: string
	labelClassName?: string
	icon?: LucideIcon
	onSelected?: (value: Values) => void
}) {
	const options = data.map(({ value, label }) => (
		<option key={value} value={value}>
			{label}
		</option>
	))

	const onChange = (e: React.ChangeEvent<HTMLSelectElement>) =>
		onSelected?.(e.target.value as Values)

	return (
		<div className={clsx('form-control grow', labelClassName)}>
			{label && (
				<div className="label">
					<span className="label-text">{label}</span>
				</div>
			)}
			{Icon ? (
				<div className="select select-bordered flex items-center gap-2">
					<Icon className="w-6 h-6" />
					<select {...props} className={clsx('grow', className)} onChange={onChange}>
						{options}
					</select>
				</div>
			) : (
				<select
					{...props}
					className={clsx('select select-bordered', className)}
					onChange={onChange}
				>
					{options}
				</select>
			)}
		</div>
	)
}
