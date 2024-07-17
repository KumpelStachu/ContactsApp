import { useGetQuery } from '@/api/hooks'
import ContactDetails from '@/components/ContactDetails'
import { createLazyFileRoute, notFound, useParams } from '@tanstack/react-router'

export const Route = createLazyFileRoute('/contact/$contactId/')({
	component: function Contact() {
		const { contactId } = useParams({ from: '/contact/$contactId/' })
		const { data, error, isFetching } = useGetQuery('/Contact/{id}', {
			params: { path: { id: parseInt(contactId) } },
		})

		if (error) {
			notFound({ throw: true })
		}

		return (
			<div className="card bg-base-100 shadow-md">
				<div className="card-body">
					<div className="card-title space-x-2">
						Szczegóły kontaktu
						{isFetching && <span className="loading loading-dots"></span>}
					</div>
					<ContactDetails contact={data} />
				</div>
			</div>
		)
	},
})
