import { categoryTypes, Schema, translateCategoryType } from '@/api/client'
import TextInput from './TextInput'
import { CakeIcon, MailIcon, PhoneIcon } from 'lucide-react'
import Select from './Select'
import { useState } from 'react'
import { useGetQuery } from '@/api/hooks'

type CategoryType = Schema<'ContactCategory'>['type']

export default function ContactEditor<
	Initial extends Schema<'ContactDTO'> | undefined = undefined,
	ContactSchema extends 'CreateContactDTO' | 'UpdateContactDTO' = Initial extends undefined
		? 'CreateContactDTO'
		: 'UpdateContactDTO',
>({
	initialContact,
	disabled,
	hideEmail,
	error,
	onSave,
	onCancel,
}: {
	initialContact?: Initial
	disabled?: boolean
	hideEmail?: boolean
	error?: Error | null
	onSave: (contact: Schema<ContactSchema>) => void
	onCancel?: () => void
}) {
	const [categoryType, setCategoryType] = useState<CategoryType>(
		initialContact?.category.type ?? 'PERSONAL'
	)
	const { data, isLoading } = useGetQuery('/ContactCategory/search', {
		params: { query: { type: categoryType } },
	})

	const categoryData = categoryTypes.map(type => ({
		value: type,
		label: translateCategoryType(type),
	}))

	function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
		e.preventDefault()
		if (isLoading || disabled) return
		const formData = new FormData(e.currentTarget)
		const contact: Schema<ContactSchema> = {
			id: initialContact?.id ?? -1,
			firstName: formData.get('firstName') as string,
			lastName: formData.get('lastName') as string,
			email: formData.get('email') as string,
			phone: formData.get('phone') as string,
			birthDate: formData.get('birthdate') as string,
			category: {
				type: formData.get('categoryType') as CategoryType,
				name: (formData.get('categoryName') ?? '') as string,
			},
		}
		onSave(contact)
	}

	return (
		<form onSubmit={handleSubmit}>
			{error && <div className="alert alert-error">{error.message}</div>}
			<div className="flex gap-4">
				<TextInput
					label="Imię"
					name="firstName"
					defaultValue={initialContact?.firstName}
					required
				/>
				<TextInput
					label="Nazwisko"
					name="lastName"
					defaultValue={initialContact?.lastName}
					required
				/>
			</div>
			{!hideEmail && (
				<TextInput
					label="Email"
					name="email"
					type="email"
					icon={MailIcon}
					defaultValue={initialContact?.email}
					required
				/>
			)}
			<TextInput
				label="Telefon"
				name="phone"
				type="tel"
				icon={PhoneIcon}
				defaultValue={initialContact?.phone}
				required
			/>
			<TextInput
				label="Urodziny"
				name="birthdate"
				type="date"
				icon={CakeIcon}
				defaultValue={initialContact?.birthDate.split('T')[0]}
				required
			/>
			<div className="flex gap-4">
				<Select
					data={categoryData}
					defaultValue={initialContact?.category.type ?? categoryType}
					label="Typ"
					name="categoryType"
					labelClassName="max-w-[40%]"
					onSelected={setCategoryType}
				/>
				{categoryType === 'WORK' ? (
					<Select
						data={data?.items.map(({ name }) => ({ label: name, value: name })) ?? []}
						defaultValue={initialContact?.category.name}
						label="Kategoria"
						name="categoryName"
					/>
				) : categoryType === 'OTHER' ? (
					<TextInput
						label="Kategoria"
						name="categoryName"
						defaultValue={
							initialContact?.category.type === 'OTHER' ? initialContact.category.name : ''
						}
						required
					/>
				) : null}
			</div>
			<div className="flex justify-end gap-4 mt-4">
				{onCancel && (
					<button className="btn" disabled={disabled} type="button" onClick={onCancel}>
						Anuluj
					</button>
				)}
				<button className="btn btn-primary" disabled={isLoading || disabled} type="submit">
					Zapisz
				</button>
			</div>
		</form>
	)
}
