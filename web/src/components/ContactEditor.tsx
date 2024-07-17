import { categoryTypes, Schema, translateCategoryType } from '@/api/client'
import TextInput from './TextInput'
import { CakeIcon, MailIcon, PhoneIcon } from 'lucide-react'
import Select from './Select'
import { useState } from 'react'
import { useGetQuery } from '@/api/hooks'
import useController from '@/hooks/useController'
import useDebounced from './useDebounced'

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
	const email = useController(initialContact?.email)
	const debouncedEmail = useDebounced(email.value, 500)
	const [categoryType, setCategoryType] = useState<CategoryType>(
		initialContact?.category.type ?? 'PERSONAL'
	)
	const { data: checkData, isLoading: isLoadingCheck } = useGetQuery(
		'/Contact/check',
		{ params: { query: { email: debouncedEmail } } },
		{ enabled: !!debouncedEmail && !hideEmail, retry: false }
	)
	const { data: categories, isLoading } = useGetQuery('/ContactCategory/search', {
		params: { query: { type: categoryType } },
	})

	const categoryData = categoryTypes.map(type => ({
		value: type,
		label: translateCategoryType(type),
	}))

	const canSubmit =
		!isLoading && !isLoadingCheck && !disabled && (!checkData?.registered || hideEmail)

	function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
		e.preventDefault()
		if (!canSubmit) return
		const formData = new FormData(e.currentTarget)
		const contact: Schema<ContactSchema> = {
			id: initialContact?.id ?? -1,
			firstName: formData.get('firstName') as string,
			lastName: formData.get('lastName') as string,
			email: hideEmail ? initialContact?.email ?? '' : (formData.get('email') as string),
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
					className={checkData?.registered ? 'input-error' : ''}
					required
					{...email.props}
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
						data={categories?.items.map(({ name }) => ({ label: name, value: name })) ?? []}
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
			<div className="flex items-center justify-between gap-4 mt-4">
				<div className="text-error">
					{!hideEmail && debouncedEmail && checkData?.registered && 'Email jest już zajęty'}
				</div>
				<div className="space-x-4">
					{onCancel && (
						<button className="btn" disabled={disabled} type="button" onClick={onCancel}>
							Anuluj
						</button>
					)}
					<button className="btn btn-primary" disabled={!canSubmit} type="submit">
						Zapisz
					</button>
				</div>
			</div>
		</form>
	)
}
