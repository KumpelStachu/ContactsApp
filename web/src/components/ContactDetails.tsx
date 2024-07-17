import { Schema, translateCategoryType } from '@/api/client'
import clsx from 'clsx'
import { CakeIcon, MailIcon, NotepadTextIcon, PhoneIcon } from 'lucide-react'

export default function ContactDetails({
	contact,
	centered,
	vertical,
}: {
	contact?: Schema<'ContactDTO'>
	centered?: boolean
	vertical?: boolean
}) {
	return (
		<div
			className={clsx(
				'flex gap-4',
				centered && 'items-center justify-center',
				vertical && 'flex-col'
			)}
		>
			<div>
				<div className="avatar placeholder">
					<div className="bg-neutral text-neutral-content w-24 h-24 rounded-full">
						{contact ? (
							<span className="text-3xl">{contact.firstName[0] + contact.lastName[0]}</span>
						) : (
							<div className="skeleton w-full h-full"></div>
						)}
					</div>
				</div>
			</div>
			<div className="space-y-1">
				<div className="mb-2 text-xl font-semibold">
					{contact ? (
						`${contact.firstName} ${contact.lastName}`
					) : (
						<div className="flex gap-2">
							<div className="skeleton w-20 h-6"></div>
							<div className="skeleton w-28 h-6"></div>
						</div>
					)}
				</div>
				<div className="flex items-center gap-2">
					<MailIcon className="w-4 h-4" />
					{contact ? (
						<a className="link link-hover" href={`mailto:${contact.email}`}>
							{contact.email}
						</a>
					) : (
						<div className="skeleton w-40 h-6"></div>
					)}
				</div>
				<div className="flex items-center gap-2">
					<PhoneIcon className="w-4 h-4" />
					{contact ? (
						<a className="link link-hover" href={`tel:${contact.phone}`}>
							{contact.phone}
						</a>
					) : (
						<div className="skeleton w-32 h-6"></div>
					)}
				</div>
				<div className="flex items-center gap-2">
					<CakeIcon className="w-4 h-4" />
					{contact ? (
						<span>{contact.birthDate.split('T')[0]}</span>
					) : (
						<div className="skeleton w-28 h-6"></div>
					)}
				</div>
				<div className="flex items-center gap-2">
					<NotepadTextIcon className="w-4 h-4" />
					{contact ? (
						<span>
							{translateCategoryType(contact.category.type)}
							{contact.category.name && ` - ${contact.category.name}`}
						</span>
					) : (
						<div className="skeleton w-36 h-6"></div>
					)}
				</div>
			</div>
		</div>
	)
}
