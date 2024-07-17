import clsx from 'clsx'
import { LucideIcon } from 'lucide-react'
import { DetailedHTMLProps, InputHTMLAttributes } from 'react'

export default function TextInput({
	icon: Icon,
	label,
	placeholder,
	className,
	labelClassName,
	...props
}: DetailedHTMLProps<InputHTMLAttributes<HTMLInputElement>, HTMLInputElement> & {
	label?: string
	icon?: LucideIcon
	labelClassName?: string
}) {
	return (
		<label className={clsx('form-control grow', labelClassName)}>
			{(label ?? placeholder) && (
				<div className="label">
					<span className="label-text">{label ?? placeholder}</span>
				</div>
			)}
			{Icon ? (
				<div className="input input-bordered flex items-center gap-2">
					<Icon className="w-6 h-6" />
					<input
						type="text"
						className={clsx('grow', className)}
						placeholder={placeholder}
						{...props}
					/>
				</div>
			) : (
				<input
					type="text"
					placeholder={placeholder}
					className={clsx('input input-bordered w-full', className)}
					{...props}
				/>
			)}
		</label>
	)
}
